# Create a SQL Server instance and database for development.
# This container is not intended for production use, connect to PaaS database instead.
# See https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&pivots=cs1-bash

FROM mcr.microsoft.com/mssql/server:2019-latest
ENV ACCEPT_EULA=Y
ENV TZ=UTC
ENV MSSQL_PID=Developer
EXPOSE 1433
COPY /setup.sh .
COPY /setup.sql .
COPY /start.sh .
CMD /start.sh
