#!/bin/sh

curl --insecure -X POST -H "Content-Type: application/json" \
    -d '{"firstName": "David", "lastName": "Woldrich", "age": 26}' \
    https://localhost:7295/woldrich/customer
