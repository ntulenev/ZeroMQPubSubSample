# ZeroMQPubSubSample

Example of using NetMQ ( ZeroMQ C# port) in publish–subscribe scenario.

```mermaid
graph TD
    subgraph Data Generation Service
        DG1[Data Generation Task 1]
        DGN[Data Generation Task N]
        DG1 --> Channel
        DGN --> Channel
        Channel --> NetMQPublisher[NetMQ Publisher]
    end
    
    NetMQPublisher -- tcp --> NetMQSubscriber[NetMQ Subscriber]
    
    subgraph Data Processing Service
        NetMQSubscriber --> DataProcessor[Data Processor]
    end
```

## Components

- **Data Generation Service**: Responsible for generating data through multiple tasks. These tasks push data through a channel to the NetMQ Publisher.
- **NetMQ Publisher**: Publishes the data over TCP.
- **NetMQ Subscriber**: Subscribes to the data sent by the publisher.
- **Data Processing Service**: Processes the received data from the NetMQ Subscriber.

## Configuration

### Data Generation Service

The **Data Generation Service** is configured using the following structure in the configuration file. Each generator represents a specific task responsible for generating data at a defined interval.

```json
{
  "Generators": {
    "Generator1": {
      "GenerationPeriodSeconds": "00:00:05",
      "TaskId": 1,
      "Destination": "Main"
    },
    "Generator2": {
      "GenerationPeriodSeconds": "00:00:15",
      "TaskId": 2,
      "Destination": "Main"
    }
  }
}
```

### Explanation

- **Generator1** and **Generator2**: These represent two example generators that generate data at different intervals.
- **GenerationPeriodSeconds**: The time interval between data generation events.
  - **Example**: `"00:00:05"` means a 5-second interval.
- **TaskId**: A unique identifier for the task.
  - **Example**: TaskId `1` for Generator1.
- **Destination**: The topic where the generated data will be sent.
  - **Example**: `"Main"`.


### Example

![Test run](TestRun.png)
