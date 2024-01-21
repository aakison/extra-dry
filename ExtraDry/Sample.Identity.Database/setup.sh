#!/bin/bash

sleep 5

running=0

while [ $running -eq 0 ]; do
	sleep 1
	echo "Waiting for SQL Server to finish installing..."
	/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'd3vRul3z!' -Q "SELECT DATABASEPROPERTYEX(N'Master', 'Collation')" -h -1 -o /tmp/result.txt
	if grep -q "SQL_Latin1_General_CP1_CI_AS" /tmp/result.txt; then
		echo "Finshed waiting for SQL Server to finish installing."
		running=1
	fi
done

echo "Running setup.sql..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'd3vRul3z!' -i setup.sql -v IdentityAppPassword="d3vRul3z!"

echo "Done running setup.sql."
