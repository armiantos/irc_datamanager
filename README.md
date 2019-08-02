# IRC Data Manager
 This development was suggested by our industry partners in hopes of being able to dump Big Data to our labs minimizing the transmission method using e-mail or FTP. 

 ## Overview 
 The IRC Data Manager is a package that contains two programs: an uploader and a retriever. These two programs serve essentially as a wrapper to database API's. The uploader was initially designed to continuously fetch data from OPC servers, particularly OPC-DA used by the _FluidMechatronix_. The retriever was designed to allow non-database users to browse through data in databases and fetch the data they need.
 
 The uploader program was originally written in Java using the open source  library JEasyOPC. The library was able to read the 5698 tag output from  FluidMechatronix in approximately 2 second intervals. But due to poor type conversions and false tag headings given by the library, the project was then  rewritten in C#.
 
 In the lab we have: _Cassandra, mongoDB, and MySQL_ servers. More about the findings in using these different databases in the database section in the documentation.
 
 ### App design
 The programs are WPF applications. To decouple components that make up a user interface for easier debugging and more manageable code, it's best to use the MVVM pattern designed for WPF applications. MVVM is some what similar to MVC, but  Here's a really great [blog](https://rachel53461.wordpress.com/) where you can learn the design pattern. To summarize:

 | Component | Function                                                                                                                                                       | File type |
 | --------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------- |
 | View      | Responsible to display data stored in _models_ through a user friendly interface                                                                               | .xaml     |
 | Model     | Responsible for templating data that needs to be displayed. Implements INotifyProperty to let view automatically update whenever data in the model is updated. | .cs       |
 | ViewModel | Links together the _view_ and _model_. Removes application logic or code behind from _views_ so that _views_ can be changed regardless of the model            | .cs       |

 The core component in MVVM is _Data Binding_. Data binding allows users to access and alter the data model through the view. It can be described as two way publish, subscribe design pattern.

 In these programs, the **main caller is located in App.xaml and the code behind App.xaml.cs**. These two programs have the DispatcherUnhandledException event handler to handle any global exception. 

 #### Material Design
 The programs needs to be user friendly as this is the application that our lab members will  use when we have the dedicated database server setup. The easiest way to style a WPF app is to implement Google's [_Material Design_](http://materialdesigninxaml.net/). They include pack icons and animated buttons. It is worth noting that **using the pop-up box to display a data grid/table that contains many rows is very slow**, it was for this reason that the _AddDataViewDialog_ is displayed in a different window to _mimic a pop up_, more on this later.

## Retriever - IRC Core
The IRC Core resembles the core services offered by [inmation](http://www.inmation.com/). The core is responsible for displaying and retrieving data from these databases. Each of these databases should wrap a common database abstract class so new databases can be supported just by installing the API's package and implementing the same database abstract class.

### Code walkthrough
#### Main interface
In App.xaml.cs the OnStartup override method initializes the main window (should really be called main view) and associates the main view model as the data context for main window's data binding. The main user interface is designed in _MainWindow.xaml_. Here the MainWindow binds to the `DataSources` [property](https://www.tutorialspoint.com/csharp/csharp_properties) in the MainViewModel. And for each `DataSource` in MainViewModel, there will be a view that will be used to display information related to it. 
```xaml
<ItemsControl ItemsSource="{Binding DataSources}">
    <ItemsControl.ItemsPanel>
        <ItemsPanelTemplate>
            <StackPanel Orientation="Vertical"/>
        </ItemsPanelTemplate>
    </ItemsControl.ItemsPanel>
    <ItemsControl.ItemTemplate>
        <DataTemplate DataType="{x:Type ds:DatabaseSource}">
            <views:DatabaseView/>
        </DataTemplate>
    </ItemsControl.ItemTemplate>
</ItemsControl>
```
The user control, `ItemsControl` creates a view for each item in the bound item source. Here for each DataSource of type `DatabaseSource` will be assigned a _DatabaseView_. The ItemsPanelTemplate is used to determine how each of these views will be stacked.

#### Database DataSource
All datasources should inherit the DataSource abstract class. The abstract class simply contains a single public property `Label` used to identify a datasource. 