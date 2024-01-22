#!/bin/bash

# Read passwords from User Secrets in mounted directory
cd /home/app/.microsoft/usersecrets/Sample.Identity-c7ee04d3
dba_password=$(grep -Po 'IdentityDbaPassword":.*?"\K.*(?=")' secrets.json)
app_password=$(grep -Po 'IdentityAppPassword":.*?"\K.*(?=")' secrets.json)
cd /

# Wait for SQL Server to finish installing
running=0
while [ $running -eq 0 ]; do
	sleep 1
	echo "Waiting for SQL Server to finish installing..."
	/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $dba_password -Q "SELECT DATABASEPROPERTYEX(N'Master', 'Collation')" -h -1 -o /tmp/result.txt
	if grep -q "SQL_Latin1_General_CP1_CI_AS" /tmp/result.txt; then
		echo "Finished waiting for SQL Server to finish installing."
		running=1
	fi
done

# Run setup.sql to finish infrastructure setup
echo "Running setup.sql..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $dba_password -i setup.sql -v IdentityAppPassword=$app_password

echo "Done running setup.sql."
