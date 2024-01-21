# Create a SQL Server instance and database for development.
# This container is not intended for production use, connect to PaaS database instead.

FROM mcr.microsoft.com/mssql/server:2019-latest
ENV ACCEPT_EULA=Y
ENV TZ=UTC
ENV MSSQL_SA_PASSWORD=d3vRul3z!
ENV MSSQL_PID=Developer
EXPOSE 1433
COPY setup.sql setup.sql
COPY setup.sh setup.sh
RUN /opt/mssql/bin/sqlservr & /setup.sh
#RUN /setup.sh
#ENTRYPOINT [ "/opt/mssql/bin/sqlservr" ]
CMD /setup.sh & /opt/mssql/bin/sqlservr
