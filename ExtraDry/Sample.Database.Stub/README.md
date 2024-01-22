# Sample.Database.Stub Container

This container is used to stub the database for testing purposes. It is used by the 
`Sample.Identity` container.  It is not intended to be used in production.

## Usage

Include the container in your `docker-compose.yml` file such as:

```yaml
  sample.database.stub:
    build:
      context: ./Sample.Database.Stub
      dockerfile: Dockerfile
    hostname: "sample.database.stub"
    volumes:
      - ${APPDATA}/Microsoft/UserSecrets:/home/app/.microsoft/usersecrets:ro
    ports:
      - "51433:1433"
```

Note:
  * The `hostname` must be `sample.database.stub` for the `Sample.Identity` container to find it.
  * The internal `ports` must be `1433` for the `Sample.Identity` container to find it.  The external port is `51433` allowing direct access from SQL Management Studio.  
  * The volume is used to provide the user secrets file to the container.  The user secrets file is shared with other projects in this solution.

### Confirming the container is running

Once launched, the database will take several seconds to configure itself.  Then, confirm docker is running and both accounts can access the database.

```bash
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -Q "SELECT @@VERSION" -P DbaPasswordFromUserSecrets
/opt/mssql-tools/bin/sqlcmd -S localhost -U IdentityApp -Q "SELECT @@VERSION" -P AppPasswordFromUserSecrets
```

## Design

The following files are used to configure this container.  All runtime files are designed to be idempotent 
should the container be restarted.

### Dockerfile
The docker container is based on `mcr.microsoft.com/mssql/server:2019-latest` as this is most similar to 
Azure's SQL Database.  The image is built with the environment variables to run the database in a container, 
except for the SA password.  The container's entrypoint is overridden to run the `startup.sh` script.

### startup.sh
The `startup.sh` script is used to set the SA password from the user secrets file and to call `setup.sh` 
script in the background, and finally to start the SQL Server daemon (`sqlservr`).

### setup.sh
The `setup.sh` script is used to create the database instance and create a application level user login.
The passwords are first read from the user secrets file.  This happens concurrent with the starting of the 
dbms engine, so a query is attempted every second to wait for the engine to start.  Once the engine is started,
the database script is run to create the database and the application user login.

### setup.sql
Once the `setup.sh` determines the database engine is running, it runs the `setup.sql` script to create the
dbms user, database instance, instance login, instance roles, and member assignments for the `IdentityApp` user.

### User Secrets
The users secrets are stored in the `%AppData%\Microsoft\UserSecrets\Sample.Identity-c7ee04d3\secrets.json` file.  
The following keys are used: 
```JSON
{
  "AppConfiguration:IdentityDbaPassword": "ComplexPassword1SuchAsAGuid",
  "AppConfiguration:IdentityAppPassword": "ComplexPassword2SuchAsAGuid"
}
```
Note that SQL Server has default complexity requirements of 8 characters, upper, lower, number, and symbol.

## Debugging

Dockerfile can be run standalone to debug the container.  In order to do so, the container must be
run from the docker command line as follows.  Visual Studio does not support debugging containers directly.

```bash
docker build -t sample.database.stub .
docker run -it --rm -p 51433:1433 sample.database.stub
```

Or with other components:

```bash
docker compose build
docker compose up 
```

Database can be connected through SSMS running on the host machine.  The connection requires:
  * Protocol: TCP
  * Servername: "localhost,51433"
  * User ID: "sa" or "IdentityApp"
  * Password: Taken from the associated user secrets file.
