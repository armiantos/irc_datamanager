
# Hadoop
Hadoop is the distributed computing framework based off of Google's file system (GFS) and Google's Map Reduce research papers. Hadoop allows big data analytic tasks to be distributed across multiple machines instead of having one super computer. This approach allows for massive scaling in the future. 

Currently, a Hadoop 3.1.2 is installed in computers 192.168.1.31 and 192.168.1.26 (running 64 bit Windows). [Single cluster setup (pseudodistributed mode guide)](https://hadoop.apache.org/docs/r3.1.2/hadoop-project-dist/hadoop-common/SingleCluster.html). Hadoop is usually installed in a UNIX environment with lots of commodity hardware to maximize parallel computation, but due to constraints, we had to setup a Windows cluster instead. I tried to use VM's to run Ubuntu but the VM's ran extremely slow.

## Installation

Make sure that JAVA_HOME is set in your environment and does not contain any spaces. If your default Java installation directory has spaces then you must use the Windows 8.3 Pathname instead e.g. `c:\Progra~1\Java\... instead of c:\Program Files\Java\....`

Hadoop is currently installed in `C:\hdc`. 

1. Download binaries [from ASF Hadoop download page](http://mirrors.ibiblio.org/apache/hadoop/common/) (e.g. `hadoop-3.2.0.tar.gz`). Otherwise, build from source, but has lots of dependencies. 
    - ***The included binaries do not include _winutils.exe_*** which is required for Hadoop, download the binaries from [cdarlint/winutils](https://github.com/cdarlint/winutils) finding the appropriate version. Place these files in the binaries folder of Hadoop (i.e. `C:\deploy\bin`)
2. The [_Starting a Single Node (pseudo-distributed) Cluster_](https://cwiki.apache.org/confluence/display/HADOOP2/Hadoop2OnWindows#Hadoop2OnWindows-StartingaSingleNode(pseudo-distributed)Cluster) section in the official [wikipage](https://cwiki.apache.org/confluence/display/HADOOP2/Hadoop2OnWindows) was used as reference. I discarded a few configuration properties in the xml files as it runs fine without it. In summary here are my changes:
    - core-site.xml
        ```xml
        <configuration>
            <property>
                <name>fs.defaultFS</name>
                <value>hdfs://localhost:9000</value>
            </property>
        </configuration>
        ```
    - hdfs-site.xml
        ```xml
        <configuration>
            <property>
                <name>dfs.replication</name>
                <value>1</value>
            </property>
        </configuration>
        ```
    - mapred-site.xml
        ```xml
        <configuration>
            <property>
                <name>mapreduce.framework.name</name>
                <value>yarn</value>
            </property>
            <property>
                <name>mapreduce.jobhistory.address</name>
                <value>localhost:10020</value>
            </property>
            <property>
                <name>mapreduce.jobhistory.webapp.address</name>
                <value>localhost:19888</value>
            </property>
        </configuration>
        ```
    - yarn-site.xml
        ```xml
        <configuration>
            <property>
                <name>yarn.nodemanager.aux-services</name>
                <value>mapreduce_shuffle</value>
            </property>

            <property>
                <name>yarn.log-aggregate-enable</name>
                <value>true</value>
            </property>

            <property>
                <name>yarn.log.server.url</name>
                <value>http://localhost:19888/jobhistory/logs</value>
            </property>
        </configuration>
        ```
        note that you may set `yarn.log-aggregate-enable` to `false` but this will prevent you from reading the logs from the web UI.
3. To start the file system, run an ***administrator command prompt*** (preferrably cmd), navigate to the installation directory (here it is `C:\deploy`), and run
    ```batch
    .\sbin\start-dfs.cmd
    ```
4. To start _YARN_, in an administrator command prompt, navigate to the installation directory (can use old prompt), and run
    ```batch
    .\sbin\start-yarn.cmd
    ```
5. Optionally if you have set log-aggregate-enable to true, you need to run the _JobHistoryServer_. Which can be run using
    ```batch
    .\bin\mapred historyserver
    ```

