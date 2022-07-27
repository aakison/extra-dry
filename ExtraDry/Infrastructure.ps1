#
# Set up environment for Sample for ETL data warehouse using pseudo-template
#

#az login

$env = "dev"

$json = Get-Content infrastructure.json
$configuration = $json | convertFrom-Json 

#
# Helper Functions
#
function Generate-Password($length) {
    $rng = New-Object System.Security.Cryptography.RNGCryptoServiceProvider;
    # Remove some characters like ", ', &, -, _, @, #, ( and ) because they negatively affect powershell scripts or SQL or AD or something...
    $passwordChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
    $buffer = [System.Byte[]]::new($length - 1)
    $rng.GetBytes($buffer)
    $password = ""
    foreach($c in $buffer) {
      $password = $password + $passwordChars[$c % $passwordChars.Length]
    }
    # add punctuation to get around needs punc rules, even though we've got good entropy without it.
    $password = $password + "!" 
    $password
}

function Get-LocalIPAddress() {
    (Invoke-WebRequest -uri "http://ifconfig.me/ip").Content
}

function Get-Secret($name) {
    $eName = Expand-Variable $name
    foreach($secret in $configuration.keyVault.secrets) {
        $eSecretName = Expand-Variable $secret.name
        if($eSecretName -eq $eName) {
            $secret.value
        }
    }
}

function Get-AspNetEnvironment($name) {
    $value = Expand-Variable $name
    if($value -eq 'dev') {
        "Development"
    }
    else {
        "Production"
    }
}

function Get-StorageConnectionString($name) {
    $eName = Expand-Variable $name
    foreach($storage in $configuration.storageAccounts) {
        $eStorageName = Expand-Variable $storage.name
        if($eStorageName -eq $eName) {
            $storage.connectionString
        }
    }
}

function Get-ConnectionStringTemplate($serverName) {
    $eServerName = Expand-Variable $serverName
    foreach($database in $configuration.dbms.databases) {
        $eDatabaseName = Expand-Variable $database.name
        if($eDatabaseName -eq $eServerName) {
            $database.connectionString
        }
    }
}

function Get-SqlConnectionString($serverName, $username, $password) {
    $template = Get-ConnectionStringTemplate $serverName
    #$username = Get-Secret $usernameKey
    #$password = Get-Secret $passwordKey
    $template.Replace("<username>", $username).Replace("<password>", $password);
}

function Get-ServiceBusConnectionString($ruleName) {
    $eRuleName = Expand-Variable $ruleName
    foreach($rule in $configuration.serviceBus.authorization) {
        $expandedRuleName = Expand-Variable $rule.name
        if($expandedRuleName -eq $eRuleName) {
            $rule.connectionString
        }
    }
}

function Expand-Variable($value) {
    if($value.EndsWith("}")) {
        $expr = $value.Trim("{}")
        $value = Invoke-Expression $expr
    }
    elseif($value.Contains("`$")) {
        $expr = "`"$value`""
        $value = Invoke-Expression $expr
    }
    $value
}

function Log-Output($message) {
    $now = Get-Date
    $elapsed = ($now - $scriptStart).ToString().Substring(3, 5)
    Write-Output "$elapsed $message"
}
$scriptStart = Get-Date

#
# Configuration Methods
#
function Configure-Subscription() {
    $name = $configuration.subscription.name
    $sub = az account show -s $name | ConvertFrom-Json
    if($sub) {
        Add-Member -InputObject $configuration.subscription -NotePropertyName az -NotePropertyValue $sub
        $_ = az account set -s $name
        $user = $sub.user.name
        Log-Output "Subscription $name set, using credentials for $user"
    }
    else {
        Log-Output "Subscription $name not found"
    }
}

function Configure-Group() {
    $name = Expand-Variable $configuration.group.name
    Log-Output "Configuring Resource Group '$name'"
    $result = az group show --name $name 2>&1
    if($result[0].ToString().Contains("ResourceGroupNotFound")) {
        Log-Output "  Resource Group not found, creating..."
        $result = az group create --name $name --location $configuration.location | ConvertFrom-Json
    }
    else {
        Log-Output "  Resource Group found."
        $result = $result | ConvertFrom-Json
    }
    Add-Member -InputObject $configuration.group -NotePropertyName az -NotePropertyValue $result
}

function Configure-KeyVault() {
    $vault = Expand-Variable $configuration.keyVault.name
    Log-Output "Configuring Key Vault '$vault'"

    $result = az keyvault show --name $vault 2>&1
    if($result.ToString().Contains("not found")) {
        Log-Output "  Vault not found, creating..."
        $location = Expand-Variable $configuration.location
        $group = Expand-Variable $configuration.group.name
        $result = az keyvault create --location $location --resource-group $group --name $vault | ConvertFrom-Json
    }
    else {
        Log-Output "  Vault found."
        $result = $result | ConvertFrom-Json
    }
    Add-Member -InputObject $configuration.keyVault -NotePropertyName az -NotePropertyValue $result

    foreach($secret in $configuration.keyVault.secrets) {
        $name = Expand-Variable $secret.name
        Log-Output "    Syncing secret '$name'"
        $result = az keyvault secret show --vault-name $vault --name $name 2>&1
        if($result.ToString().Contains("SecretNotFound")) {
            Log-Output "      Secret not found, creating..."
            $value = Expand-Variable $secret.default
            $contents = Expand-Variable $secret.contents
            $newSecret = az keyvault secret set --vault-name $vault --name $name --value $value --description $contents | ConvertFrom-Json
            Add-Member -InputObject $secret -NotePropertyName az -NotePropertyValue $newSecret
            Add-Member -InputObject $secret -NotePropertyName value -NotePropertyValue $newSecret.value
        }
        else {
            $sec = $result | ConvertFrom-Json
            Log-Output "      Secret found"
            Add-Member -InputObject $secret -NotePropertyName az -NotePropertyValue $sec
            Add-Member -InputObject $secret -NotePropertyName value -NotePropertyValue $sec.value
        }
    }
}

function Configure-Storage() {
    Log-Output "Configuring Storage Accounts"
    $group = Expand-Variable $configuration.group.name
    foreach($account in $configuration.storageAccounts) {
        $name = (Expand-Variable $account.name).Replace("-", "") # kebab case not allowed.
        Log-Output "  Configuring Storage Account '$name'"
        $result = $azureStorage = az storage account show --name $name 2>&1
        if($result.ToString().Contains("not found")) {
            Log-Output "    Storage account not found, creating..."
            $result = az storage account create --resource-group $group --name $name --https-only true --sku Standard_GRS | ConvertFrom-Json
        }
        else {
            Log-Output "  Storage account $name retrieved"
            $result = $result | ConvertFrom-Json
        }
        Add-Member -InputObject $account -NotePropertyName az -NotePropertyValue $result
        $result = az storage account show-connection-string --name $name | convertFrom-Json
        Add-Member -InputObject $account -NotePropertyName connectionString -NotePropertyValue $result.connectionString
    }
}

function Configure-ServiceBus() {
    $group = Expand-Variable $configuration.group.name
    $bus = $configuration.serviceBus
    $name = Expand-Variable $bus.name
    Log-Output "Configuring Service Bus '$name'"
    $result = az servicebus namespace show --resource-group $group --name $name 2>&1
    if($result[0].ToString().Contains("ResourceNotFound")) {
        Log-Output "  Service bus not found, creating..."
        $sku = $bus.sku
        $location = $configuration.location
        $result = az servicebus namespace create --resource-group $group --name $name --sku $sku --location $location 2>&1
    }
    else {
        Log-Output "  Service bus retrieved"
        $result = $result | ConvertFrom-Json
    }
    Add-Member -InputObject $bus -NotePropertyName az -NotePropertyValue $result
    
    foreach($queue in $bus.queues) {
        $queueName = $queue.name
        Log-Output "  Configuring queue '$queueName'"
        $result = az servicebus queue show --resource-group $group --namespace-name $name --name $queueName 2>&1
        if($result[0].ToString().Contains("NotFound")) {
            Log-Output "    Queue not found, creating..."
            $result = az servicebus queue create --resource-group $group --namespace-name $name --name $queueName 2>&1 | ConvertFrom-Json
        }
        else {
            Log-Output "    Queue retrieved"
            $result = $result | ConvertFrom-Json
        }
        Add-Member -InputObject $queue -NotePropertyName az -NotePropertyValue $result
    }

    foreach($rule in $bus.authorization) {
        $ruleName = $rule.name
        Log-Output "  Configuring Authorization Rule '$ruleName'"
        $result = az servicebus namespace authorization-rule show --resource-group $group --namespace-name $name --name $ruleName 2>&1
        if($result[0].ToString().Contains("NotFound")) {
            Log-Output "    Authorization not found, creating..."
            $send = if($rule.send) { "Send" } else { "" }
            $listen = if($rule.listen) { "Listen" } else { "" }
            $manage = if($rule.manage) { "Manage" } else { "" }
            $result = az servicebus namespace authorization-rule create --resource-group $group --namespace-name $name --name $ruleName --rights $send $listen $manage | ConvertFrom-Json
        }
        else {
            Log-Output "    Authorization retrieved"
            $result = $result | ConvertFrom-Json
        }
        Add-Member -InputObject $rule -NotePropertyName az -NotePropertyValue $result

        $result = az servicebus namespace authorization-rule keys list --resource-group $group --namespace-name $name --name $ruleName | ConvertFrom-Json
        Add-Member -InputObject $rule -NotePropertyName keys -NotePropertyValue $result
        Add-Member -InputObject $rule -NotePropertyName connectionString -NotePropertyValue $result.primaryConnectionString
    }
}

function Get-CreateUserSql($username, $password) {
@"
    IF NOT EXISTS (SELECT [Name] FROM [sys].[sql_logins] WHERE [Name] = '$username') 
    BEGIN
        CREATE LOGIN [$username] WITH PASSWORD = '$password'
    END
    ELSE
    BEGIN
        ALTER LOGIN [$username] WITH PASSWORD = '$password'
    END
"@
}

function Get-AddExecutorRoleSql() {
@"
    IF DATABASE_PRINCIPAL_ID('db_executor') IS NULL
    BEGIN
        CREATE ROLE [db_executor]
        GRANT EXECUTE TO [db_executor]
    END
"@
}

function Get-AddRoleToUserSql($username, $roles) {
    $sql = @"
        IF NOT EXISTS (SELECT [Name] FROM [sys].[sysusers] WHERE [Name] = '$username') 
        BEGIN
            CREATE USER [$username] FROM LOGIN [$username]
        END`n
"@

    foreach($role in $roles) {
        $roleSql = "ALTER ROLE [$role] ADD MEMBER [$username]`n"
        $sql = "$sql $roleSql"
    }
    $sql
}

function Invoke-Sql($server, $database, $sql) {
    #https://docs.microsoft.com/en-us/sql/tools/sqlcmd-utility?view=sql-server-ver16
    $server = $configuration.dbms.az.fullyQualifiedDomainName
    $username = Expand-Variable $configuration.dbms.username
    $password = Expand-Variable $configuration.dbms.password
    #Write-Output $server $database $username $password $sql
    if($database) {
        $result = sqlcmd -S $server,1433 -d $database -U $username -P $password -r -h-1 -Q $sql
    }
    else {
        $result = sqlcmd -S $server,1433 -U $username -P $password -r -h-1 -Q $sql
    }
    $result
}

function Configure-SqlServer() {
    $name = (Expand-Variable $configuration.dbms.name).ToLower()
    $group = if($configuration.dbms.group) { $configuration.dbms.group } else { $configuration.group.name }
    $group = Expand-Variable $group
    $location = $configuration.location
    $username = Expand-Variable $configuration.dbms.username
    $password = Expand-Variable $configuration.dbms.password

    Log-Output "Configuring SQL Server DBMS '$name'"

    $result = az sql server show --resource-group $group --name $name 2>&1
    if($result[0].ToString().Contains("ResourceNotFound")) {
        Log-Output "  DBMS not found, creating..."
        $result = az sql server create --resource-group $group --name $name --admin-user $username --admin-password $password --location $location
        $result = $result[1..$result.Count]
        $result = $result | ConvertFrom-Json
    }
    else {
        Log-Output "  DBMS found."
        $result = $result | ConvertFrom-Json
    }
    Add-Member -InputObject $configuration.dbms -NotePropertyName az -NotePropertyValue $result

    foreach($rule in $configuration.dbms.firewall) {
        $ruleName = Expand-Variable $rule.name
        Log-Output "    Syncing firewall rule '$ruleName'"
        $result = az sql server firewall-rule show --resource-group $group --server $name --name $ruleName 2>&1
        $value = Expand-Variable $rule.value
        if($result[0].ToString().Contains("ResourceNotFound")) {
            Log-Output "      No firewall rule found, creating..."
            $result = az sql server firewall-rule create --resource-group $group --server $name --name $ruleName --start-ip-address $value --end-ip-address $value | ConvertFrom-Json
        }
        else {
            Log-Output "      Firewall rule found."
            $result = $result | ConvertFrom-Json
        }
        Add-Member -InputObject $rule -NotePropertyName az -NotePropertyValue $result
    }

    foreach($user in $configuration.dbms.users) {
        $username = Expand-Variable $user.name
        $password = Expand-Variable $user.password
        $sql = Get-CreateUserSql $username $password
        Log-Output "    Creating user '$username'... (updating password if already exists)"
        $result = Invoke-Sql $name $null $sql 2>&1
    }
}

function Configure-SqlDatabases() {
    $serverName = Expand-Variable $configuration.dbms.name
    foreach($database in $configuration.dbms.databases) {
        $name = Expand-Variable $database.name
        $group = if($database.group) { $database.group } else { $configuration.group.name }
        $group = Expand-Variable $group
        Log-Output "Configuring SQL Database '$name'"
        $result = az sql db show --resource-group $group --server $serverName --name $name 2>&1
        if($result[0].ToString().Contains("ResourceNotFound")) {
            Log-Output "  Database not found, creating..."
            $size = "S0" # small 10 DTUs
            $result = az sql db create --resource-group $group --server $serverName --name $name --service-objective $size | ConvertFrom-Json
        }
        else {
            Log-Output "  Database found."
            $result = $result | ConvertFrom-Json   
        }
        Add-Member -InputObject $database -NotePropertyName az -NotePropertyValue $result

        Log-Output "    Reading ADO.NET Connection String"        
        $result = az sql db show-connection-string --server $serverName --name $name --client ado.net --auth-type SqlPassword | ConvertFrom-Json
        Add-Member -InputObject $database -NotePropertyName connectionString -NotePropertyValue $result

        Log-Output "    Adding db_executor role"
        $sql = Get-AddExecutorRoleSql
        $result = Invoke-Sql $serverName $name $sql 2>&1

        foreach($user in $database.users) {
            $username = $user.username
            Log-Output "    Adding roles to $username"
            $roles = $user.roles
            $sql = Get-AddRoleToUserSql $username $roles
            Invoke-Sql $serverName $name $sql
        }
    }
}

function Configure-AppServicePlan() {
    Log-Output "Configuring AppServicePlan"
    $name = Expand-Variable $configuration.appServicePlan.name
    $group = if($configuration.appServicePlan.group) { $configuration.appServicePlan.group } else { $configuration.group.name }
    $group = Expand-Variable $group
    $result = az appservice plan show --resource-group $group --name $name 2>&1
    if($result.ToString().Contains("ResourceNotFound")) {
        Log-Output "  App service plan '$name ($group)' not found"

    }
    else {
        Log-Output "  App service plan '$name' found"
        $result = $result | ConvertFrom-Json 
    }
    Add-Member -InputObject $configuration.appServicePlan -NotePropertyName az -NotePropertyValue $result
}

function Set-ConnectionString($group, $name, $type, $key, $value) {
    $quotedString = "$key=""" + $value.Replace("""", """""") + """"
    az webapp config connection-string set `
        --resource-group $group --name $name `
        --connection-string-type $type --settings $quotedString
}

function Set-EnvironmentVariable($group, $name, $key, $value) {
    az webapp config appsettings set `
        --resource-group $group --name $name `
        --settings $key=$value
}

function Configure-AppService() {
    $name = Expand-Variable $configuration.appServicePlan.appService.name
    Log-Output "Configuring AppService '$name'"
    $group = Expand-Variable $configuration.group.name
    $result = az webapp show --resource-group $group --name $name 2>&1
    if($result[0].ToString().Contains("ResourceNotFound")) {
        Log-Output "  App service not found, creating..."
        $result = az webapp create --resource-group $group --plan $configuration.appServicePlan.az.id --name $name | ConvertFrom-Json
    }
    else {
        Log-Output "  App service found."
        $result = $result | ConvertFrom-Json
    }
    Add-Member -InputObject $configuration.appServicePlan.appService -NotePropertyName az -NotePropertyValue $result

    foreach($connectionString in $configuration.appServicePlan.appService.connectionStrings) {
        $key = Expand-Variable $connectionString.name
        $type = if($connectionString.type) { $connectionString.type } else { "Custom" }
        $value = Expand-Variable $connectionString.value

        Log-Output "    Setting connection string '$key'"
        $result = Set-ConnectionString $group $name $type $key $value
        Add-Member -InputObject $connectionString -NotePropertyName az -NotePropertyValue $result
    }

    foreach($environmentVariable in $configuration.appServicePlan.appService.environmentVariables) {
        $key = Expand-Variable $environmentVariable.name
        $value = Expand-Variable $environmentVariable.value

        Log-Output "    Setting environment variable '$key'"
        $result = Set-EnvironmentVariable $group $name $key $value
        Add-Member -InputObject $environmentVariable -NotePropertyName az -NotePropertyValue $result
    }
}

function Configure-All() {
    Configure-Subscription
    Configure-Group
    Configure-KeyVault
    Configure-Storage
    Configure-ServiceBus
    Configure-SqlServer
    Configure-SqlDatabases
    Configure-AppServicePlan
    Configure-AppService
}

