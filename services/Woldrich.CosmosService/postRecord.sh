#!/bin/sh

curl --insecure -vv -H "Content-Type: application/json" \
    -d '{"firstName": "David", "lastName": "Woldrich", "age": 26}' \
    https://cosmosdb:7295/woldrich/customer
