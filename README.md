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


### Example

![Test run](TestRun.png)
