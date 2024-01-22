#!/bin/bash

cd /home/app/.microsoft/usersecrets/Sample.Identity-c7ee04d3
dba_password=$(grep -Po 'IdentityDbaPassword":.*?"\K.*(?=")' secrets.json)
export MSSQL_SA_PASSWORD=$dba_password
cd /

/setup.sh &

/opt/mssql/bin/sqlservr
