- TODO
  - read the calling service request header
    - contains the service name?
  
  - check which service is calling and match to a price list for services
  - then check customerId's services and apply modifiers for the date range?

  - Model
    - ServicePlan
      - id = long
      - Service = Service
      - start = datetime

    - Service
      - id = long
      - name = string
      - price = float
      - dayrange = int[2]
      - Discount = Discount

    - Discount
      - id = long
      - amount = int
      - serviceName = string
      - start = datetime
      - end = datetime
    
    - Customer
      - id = long
      - ServicePlans = list<ServicePlan>
      - freedays = int

reuest = (customerId, start, end)
header = caller: "service_a | service_b | service_c"

var customer = GetCustomer(customerId)
var customerServicePlans = customer.ServicePlans.filter(s => s.service.name == caller && s.start <= start)

in loop
if date isBetween serviceplan.service.range[0] and range[1]
if i < freedays then price = 0
if serviceplan.service.name is contained inside customer.discounts-any discount.serviceName
 and date isbetween discount.start and discount.end
 then price = serviceplan.service.price * (discount.amount / 10)

totalPrice = totalPrice + price

return totalPrice


```
 __        ___  ___       __             __
/  \ |  | |__  |__  |\ | /__` |     /\  |__)
\__X \__/ |___ |___ | \| .__/ |___ /~~\ |__)
```

# **Backend - Evaluation Assignment - Pricing calculator**

## **Background**

Company X provides services to other companies in the region. Your job is to develop a Web API within a micro-services solution that is solely responsible for calculating prices and should only be called by other services, not humans. There are three types of services; _Service A_, _Service B_ and _Service C_. These services have different prices and the prices also depend on the customer, the time period for which they are charged, possible discount (percentage of total price) and free days. A customer can choose which service they want to use independently of other services and customers.

## **User story**

As a calling service it should be possible to call an endpoint with customerId, start and end in PricingService to know what to charge for a specific customer

## **Requirements**

Build this Web API (PricingService) with appropriate endpoints and implement the following requirements:

- Base costs are as follows:

  - _Service A_ = € 0,2 / working day (monday-friday)
  - _Service B_ = € 0,24 / working day (monday-friday)
  - _Service C_ = € 0,4 / day (monday-sunday)

- Each customer can have specific prices for each service (e.g. _Customer A_ only pays € 0,15 per working day for _Service A_ but pays € 0,25 per working day for _Service B_).

- Customers can have discounts for each service
- Each customer can have a start date for each service
- Each customer can have a number of free days which are global for all services

# Testing

Use appropriate methodology to test the following scenarios:

## **Test case 1**

_Customer X_ started using _Service A_ and _Service C_ 2019-09-20. _Customer X_ also had an discount of 20% between 2019-09-22 and 2019-09-24 for _Service C_. What is the total price for _Customer X_ up until 2019-10-01?

1600 / 1000 = 1.6€
3600 / 1000 = 3.6€
(1600 + 3600) / 1000 = 5.2€

## **Test case 2**

_Customer Y_ started using _Service B_ and _Service C_ 2018-01-01. _Customer Y_ had 200 free days and a discount of 30% for the rest of the time. What is the total price for _Customer Y_ up until 2019-10-01?

## **Technologies**

.NET (.NET Core 3.1/.NET 5), Python 3
