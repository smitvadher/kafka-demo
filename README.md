# Kafka Demo
Kafka demo in .NET 8, showcasing the use of Confluent's Kafka Client. The demo includes two APIs: OrderAPI and PaymentAPI. The Payment API captures payment transaction request and produces Kafka messages of the transaction result. The Order API consumes the payment transaction messages. Please note that the APIs do not have the logic to capture payments or process orders; they serve as examples of Kafka message handling.

## Tools Required
- [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Apache Kafka](https://kafka.apache.org/downloads)
- [Java Runtime Environment (JRE)](https://www.java.com/en/download/)

## Usage

Before running the application, ensure that Apache Kafka is running using the following commands:

```bash
.\bin\windows\zookeeper-server-start.bat .\config\zookeeper.properties
.\bin\windows\kafka-server-start.bat .\config\server.properties
```

Follow these steps to build and run the APIs:

```bash
# Build and run OrderAPI
cd src\Services\OrderAPI
dotnet build
dotnet run

# Build and run PaymentAPI
cd src\Services\PaymentAPI
dotnet build
dotnet run
```


To send a payment request, use the following example commands or code snippets:
```bash
# Send payment request using curl or any API testing tool
curl -X POST http://localhost:5000/api/payment -d '{}'
```
