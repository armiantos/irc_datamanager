# IRC Data Manager
 This development was suggested by our industry partners in hopes of being able to dump Big Data to our labs minimizing the transmission method using e-mail or FTP. 

 ## Overview 
 The IRC Data Manager is a package that contains two programs: an uploader and a retriever. These two programs serve essentially as a wrapper to database API's. The uploader was initially designed to continuously fetch data from OPC servers, particularly OPC-DA used by the _FluidMechatronix_. The retriever was designed to allow non-database users to browse through data in databases and fetch the data they need.
 
 The uploader program was originally written in Java using the open source  library JEasyOPC. The library was able to read the 5698 tag output from  FluidMechatronix in approximately 2 second intervals. But due to poor type conversions and false tag headings (false negative quality) given by the library, the project was then  rewritten in C# which allowed less than 1 second retrievals and no false negative quality readings.
 
 In the lab we have: _Cassandra, mongoDB, and MySQL_ servers. More about the findings in using these different databases in the database section in the documentation.