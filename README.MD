### This is the experimental application for the development of a message broker.
> The target of this app. It was used the full and complete centric message model for manage transactions. See QuantMessage.  
> - You must create an environment variable with name "Quant" and initialize it is with value "localhost".
> - Download [nats-stream-server](https://github.com/nats-io/nats-streaming-server/releases). Unzip and copy to path __"C:\nats-server"__.
> - Create subdirectory __\config__. After you need to create the localhost.conf file. 
> - Open the config file and add content into the body:
```json
# listen: localhost:4222
streaming {
    cluster_id: "<work station name>"
    id: "<work station name>"
    discover_prefix: "_TEST.<work station name>"
    store: "file"
    dir: "c:\\nats-server\\store\\"
    sd: true
    sv: true
    hb_interval: "30s" #Interval at which the server sends an heartbeat to a client
    hb_timeout: "30s" # How long the server waits for a heartbeat response from the client before considering it a failed heartbeat
    hb_fail_count: 10
    store_limits {
        max_channels: 100
        max_subs: 100
        max_msgs: 10000
        max_bytes: 1GB
        max_age: "24h"
        max_inactivity: "24h"
    }
}
```
> - Open the PoweShell. start nats-stream-server using the command __.\nats-streaming-server.exe -sc .\config\localhost.conf__.
> - Run one of the [examples](/Examples). 
> - Nats documentation: 
> - [Concepts](https://docs.nats.io/nats-streaming-concepts/intro)
> - [Developing](https://docs.nats.io/developing-with-nats-streaming/streaming)
> - [Installing](https://docs.nats.io/nats-streaming-server/install)