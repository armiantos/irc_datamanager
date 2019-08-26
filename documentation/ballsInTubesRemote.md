# ballsInTubesRemote
Send and receive signals from ballsInTubes experiment using Python remotely. The pipeline utilizes the open source messaging broker RabbitMQ. [_RabbitMQ-Matlab-Client_](https://github.com/ragavsathish/RabbitMQ-Matlab-Client) used in the 32-bit Matlab is written by _ragavsathish_. The RabbitMQ server is set-up in computer CME-712337 under OPCUSER. 


## Usage
### Starting the RabbitMQ node 192.168.1.31 (CME-712337)
1. Search _Services_ in the Window's taskbar (Cortana).
2. Look for the RabbitMQ entry in the _Services_ window.
3. If the status is not `Running` then right click on the service and click run.

### In computer 192.168.1.37
1. Open MATLAB R2011a.
2. Switch directories to `~/Documents/MATLAB/ballsInTube`.
3. Type in `mex -setup` followed by `y 1 y` in the _Command Window_.
4. Type in `start` which will launch `start.m` script in the current directory.
5. Type in `java.lang.Thread(subscriber).start` in the _Command Window_ which will run the subscriber in a different thread. This subscriber will run `myCallbackFcn` to parse the incoming messages and "return" the appropriate outputs accordingly.

### In operator computer _that has access to the lab's local network_
1. Ensure that `pika` is installed in your current Python environment. This can be installed using pip or if you're using PyCharm just add the library into your virtual environment. This library will allow you to interface with the RabbitMQ server.
2. Import the controller in _controller.py_.
    ```python
    from controller import Controller
    ```
3. Create a new instance of the controller, specifying the RabbitMQ host's address.
    ```python
    controller = Controller('192.168.1.31')
    ```
4. Refer to the methods documentation below for the list of methods you can call from the controller.

## Methods
|Method|Description|
|---|---|
|`get_fan_speeds()`| returns an array of fan speeds from the experiment by calling `getFanSpeeds` in computer `192.168.1.37`.
|`set_fan_speed(speed, tube)`| calls the `setFanSpeed` function in computer `192.168.1.37`. Speed is not in percentage, it is absolute. |
|`get_level(tube)` | returns the ultrasound sensor reading by calling `getLevel#` in computer `192.168.1.37`. Note that `getLevel#` is originally 1-indexed, but `get_level(tube)` is 0-indexed.