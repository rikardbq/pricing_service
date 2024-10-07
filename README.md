### Running

- Start the dotnet backend `>_ $ dotnet run --launch-profile http`
- Python
  - ``>_ $ pip install requests``
  - ``>_ $ python TestCases.py``
- Manual steps
  - Populate in-memory database
  - adds 2 customers, id: 1, id: 2 and a bunch of other stuff.
    - `[POST] http://localhost:5227/api/PriceCalculator/CreateTestData`
      - Headers 
        - Accept: text/plain 
        - Content-Type: application/json 
  - Calculate price for customer for given service usage
    - `[POST] http://localhost:5227/api/PriceCalculator/CalculatePrice` 
      - Headers 
        - Accept: text/plain 
        - Content-Type: application/json 
        - caller: SERVICE_A 
          - or SERVICE_B, SERVICE_C
      - Body 
        - `{ "id": 1, "start": "2019-09-20", "end": "2019-10-01" }`
- **Test scenario 1**
  - _Customer X_ started using _Service A_ and _Service C_ 2019-09-20. _Customer X_ also had an discount of 20% between 2019-09-22 and 2019-09-24 for _Service C_. What is the total price for _Customer X_ up until 2019-10-01?
- **Test scenario 2**
  - _Customer Y_ started using _Service B_ and _Service C_ 2018-01-01. _Customer Y_ had 200 free days and a discount of 30% for the rest of the time. What is the total price for _Customer Y_ up until 2019-10-01?


