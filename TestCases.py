import requests

baseUrl = "http://localhost:5227/api/PriceCalculator/"
headers = {
    "Accept": "text/plain",
    "Content-Type": "application/json"
}

def createTestData():
  print(":::::: CREATE TEST DATA ::::::")
  response = requests.post(
    baseUrl + "CreateTestData",
    headers=headers
  )
  print(response)
  if response.status_code == 200:
    print(":::::: CREATE TEST DATA DONE ::::::")
  else:
    print(":::::: CREATE TEST DATA FAILED, ALREADY EXISTS? ::::::")

# RUN THIS ONCE when the dotnet backend is running
# start it with:
#   dotnet run --launch-profile http
# 
# uncomment this to create test data
# createTestData()

headersCase1_ServiceA = {
  **headers,
  "caller": "SERVICE_A"
}
headersCase1_ServiceC = {
  **headers,
  "caller": "SERVICE_C"
}

bodyCase1 = {
  "id": 1,
  "start": "2019-09-20",
  "end": "2019-10-01"
}

print("\n:::::: GET TOTAL COST FOR CUSTOMER id: 1 ::::::")
print(bodyCase1)
response = requests.post(
  baseUrl + "CalculatePrice",
  json=bodyCase1,
  headers=headersCase1_ServiceA
)
totalCostCase1_ServiceA = response.json()

response = requests.post(
  baseUrl + "CalculatePrice",
  json=bodyCase1,
  headers=headersCase1_ServiceC
)
totalCostCase1_ServiceC = response.json()
print("\n:::::: TOTAL COST", (totalCostCase1_ServiceA + totalCostCase1_ServiceC) / 1000, "€ ::::::")

headersCase2_ServiceB = {
  **headers,
  "caller": "SERVICE_B"
}
headersCase2_ServiceC = {
  **headers,
  "caller": "SERVICE_C"
}

bodyCase2 = {
  "id": 2,
  "start": "2018-01-01",
  "end": "2019-10-01"
}

print("\n:::::: GET TOTAL COST FOR CUSTOMER id: 2 ::::::")
print(bodyCase2)
response = requests.post(
  baseUrl + "CalculatePrice",
  json=bodyCase2,
  headers=headersCase2_ServiceB
)
totalCostCase2_ServiceB = response.json()

response = requests.post(
  baseUrl + "CalculatePrice",
  json=bodyCase2,
  headers=headersCase2_ServiceC
)
totalCostCase2_ServiceC = response.json()
print("\n:::::: TOTAL COST", (totalCostCase2_ServiceB + totalCostCase2_ServiceC) / 1000, "€ ::::::")
