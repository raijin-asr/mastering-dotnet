### 1. ADO
- Active Data Objects is Microsoft Technology, Microsoft Active-X component.
- Programming interface to access data in a database and automatically installed with Microsoft IIS.


#### 1.1. Steps to access a Database:
	a. Create an ADO connection to a database
	b. Open the database connection
	c. Create an ADO recordset
	d. Open the recordset
	e. Extract the data you need from the recordset
	f. Close the recordset
	g. Close the connection

### 2. ADO Database Connection
-  Easiest way to connect to a database is to use a DSN-less connection. A DSN-less connection can be used against any Microsoft Access database on your web site.
```
<%
set conn=Server.CreateObject("ADODB.Connection")
conn.Provider="Microsoft.Jet.OLEDB.4.0"  
conn.Open "c:/webdata/northwind.mdb"
%>
```

- Provider is Microsoft Access database drive.

- Another way is to connect is to use ODBC Database Connection.
```
<%
set conn=Server.CreateObject("ADODB.Connection")
conn.Open "northwind"
%>
```

#### 2.1.  ODBC connection to an MS Access Database
- Following configuration has to be done on computer where website is located or if you run PWS(Personal Web Server) or IIS on your own computer else need to provide physical access to remote server or web host do this.

	a. Open the ODBC icon in your Control Panel.
	b. Choose the System DSN tab.
	c. Click on Add in the System DSN tab.
	d. Select the Microsoft Access Driver. Click Finish.
	e. In the next screen, click Select to locate the database.
	f. Give the database a Data Source Name (DSN).
	g. Click OK.

### 3. ADO Recordset
- Recordset stores data.
- Access to "Customer" table inside with given codde:
```
<%
set conn=Server.CreateObject("ADODB.Connection")
conn.Provider="Microsoft.Jet.OLEDB.4.0"
conn.Open "c:/webdata/northwind.mdb"

set rs=Server.CreateObject("ADODB.recordset")
rs.Open "Customers", conn
%>
```

- Access data using SQL:
```
rs.Open "Select * from Customers", conn
````

-  Given code extracts data from recordset:
```
rs.Open "Select * from Customers", conn

for each x in rs.fields
  response.write(x.name)
  response.write(" = ")
  response.write(x.value)
next
%>
```

### 4. Display data
- Code to display field name and values:
```
<%
set conn=Server.CreateObject("ADODB.Connection")
conn.Provider="Microsoft.Jet.OLEDB.4.0"
conn.Open "c:/webdata/northwind.mdb"

set rs = Server.CreateObject("ADODB.recordset")
rs.Open "SELECT * FROM Customers", conn

do until rs.EOF
  for each x in rs.Fields
    Response.Write(x.name)
    Response.Write(" = ")
    Response.Write(x.value & "<br>")
  next
  Response.Write("<br>")
  rs.MoveNext
loop

rs.close
conn.close
%>

</body>
</html>
```

- Code to display data in an HTML table:
```
rs.Open "SELECT Companyname, Contactname FROM Customers", conn
%>


<table border="1" style="width:100%;background-color:#fff5ee;">
<tr>
<%for each x in rs.Fields
  response.write("<th style='text-align:left;background-color:#b0c4de;'>" & x.name & "</th>")
  next%>
  </tr>
  <%do until rs.EOF%>
    <tr>
    <%for each x in rs.Fields%>
      <td><%Response.Write(x.value)%></td>
    <%next
    rs.MoveNext%>
    </tr>
  <%loop
  rs.close
  conn.close
  %>
</table>
```

### 5. ADO Query Data
- Display selected data:
```
<%
set conn=Server.CreateObject("ADODB.Connection")
conn.Provider="Microsoft.Jet.OLEDB.4.0"
conn.Open "c:/webdata/northwind.mdb"

set rs=Server.CreateObject("ADODB.recordset")
sql="SELECT Companyname, Contactname FROM Customers WHERE CompanyName LIKE 'A%'"
rs.Open sql, conn
%>

<table border="1" width="100%">
  <tr>
  <%for each x in rs.Fields
    response.write("<th>" & x.name & "</th>")
  next%>
  </tr>
  <%do until rs.EOF%>
    <tr>
    <%for each x in rs.Fields%>
      <td><%Response.Write(x.value)%></td>
    <%next
    rs.MoveNext%>
    </tr>
  <%loop
  rs.close
  conn.close%>
</table>
```

- create filter and let user chose country of customer:
```
set rs=Server.CreateObject("ADODB.recordset")
sql="SELECT DISTINCT Country FROM Customers ORDER BY Country"
rs.Open sql, conn

country=request.form("country")
%>

<form method="post">
Choose Country <select name="country">
<% do until rs.EOF
  response.write("<option")
  if rs.fields("country")=country then
    response.write(" selected")
  end if
  response.write(">")
  response.write(rs.fields("Country"))
  rs.MoveNext
loop
rs.Close
set rs=Nothing %>
</select>
<input type="submit" value="Show customers">
</form>

<%
if country<>"" then
  sql="SELECT Companyname,Contactname,Country FROM Customers WHERE country='" & country & "'"
  set rs=Server.CreateObject("ADODB.Recordset")
  rs.Open sql,conn
%>

<table width="100%" cellspacing="0" cellpadding="2" border="1">
<tr>
  <th>Companyname</th>
  <th>Contactname</th>
  <th>Country</th>
</tr>

<%
do until rs.EOF
  response.write("<tr>")
  response.write("<td>" & rs.fields("companyname") & "</td>")
  response.write("<td>" & rs.fields("contactname") & "</td>")
  response.write("<td>" & rs.fields("country") & "</td>")
  response.write("</tr>")
  rs.MoveNext
loop
rs.close
conn.Close
set rs=Nothing
set conn=Nothing%>
</table>
<% end if %>
```

### 6. Sort Data
- Order by companyName to display companyname and contactname
```
sql="SELECT Companyname, Contactname FROM Customers ORDER BY CompanyName"
rs.Open sql, conn
%>

<table border="1" width="100%">
  <tr>
  <%for each x in rs.Fields
    response.write("<th>" & x.name & "</th>")
  next%>
  </tr>
  <%do until rs.EOF%>
    <tr>
    <%for each x in rs.Fields%>
      <td><%Response.Write(x.value)%></td>
    <%next
    rs.MoveNext%>
    </tr>
  <%loop
  rs.close
  conn.close%>
</table>
```

### 7. Add Records
- Use SQL INSERT INTO command
- Form to input collect data:
```
<form method="post" action="demo_add.asp">
<table>
<tr>
<td>CustomerID:</td>
<td><input name="custid"></td>
</tr><tr>
<td>Company Name:</td>
<td><input name="compname"></td>
</tr><tr>
<td>Contact Name:</td>
<td><input name="contname"></td>
</tr><tr>
<td>Address:</td>
<td><input name="address"></td>
</tr><tr>
<td>City:</td>
<td><input name="city"></td>
</tr><tr>
<td>Postal Code:</td>
<td><input name="postcode"></td>
</tr><tr>
<td>Country:</td>
<td><input name="country"></td>
</tr>
</table>
<br><br>
<input type="submit" value="Add New">
<input type="reset" value="Cancel">
</form>
```

- demo_add.asp file having INSERT logic:
```
sql="INSERT INTO customers (customerID,companyname,"
sql=sql & "contactname,address,city,postalcode,country)"
sql=sql & " VALUES "
sql=sql & "('" & Request.Form("custid") & "',"
sql=sql & "'" & Request.Form("compname") & "',"
sql=sql & "'" & Request.Form("contname") & "',"
sql=sql & "'" & Request.Form("address") & "',"
sql=sql & "'" & Request.Form("city") & "',"
sql=sql & "'" & Request.Form("postcode") & "',"
sql=sql & "'" & Request.Form("country") & "')"

on error resume next
conn.Execute sql,recaffected
if err<>0 then
  Response.Write("No update permissions!")
else
  Response.Write("<h3>" & recaffected & " record added</h3>")
end if
conn.close
%>
```

### 8. Update Records
```
<html>
<body>

<h2>Update Record</h2>
<%
set conn=Server.CreateObject("ADODB.Connection")
conn.Provider="Microsoft.Jet.OLEDB.4.0"
conn.Open "c:/webdata/northwind.mdb"

cid=Request.Form("customerID")

if Request.form("companyname")="" then
  set rs=Server.CreateObject("ADODB.Recordset")
  rs.open "SELECT * FROM customers WHERE customerID='" & cid & "'",conn
  %>
  <form method="post" action="demo_update.asp">
  <table>
  <%for each x in rs.Fields%>
  <tr>
  <td><%=x.name%></td>
  <td><input name="<%=x.name%>" value="<%=x.value%>"></td>
  <%next%>
  </tr>
  </table>
  <br><br>
  <input type="submit" value="Update record">
  </form>
<%
else
  sql="UPDATE customers SET "
  sql=sql & "companyname='" & Request.Form("companyname") & "',"
  sql=sql & "contactname='" & Request.Form("contactname") & "',"
  sql=sql & "address='" & Request.Form("address") & "',"
  sql=sql & "city='" & Request.Form("city") & "',"
  sql=sql & "postalcode='" & Request.Form("postalcode") & "',"
  sql=sql & "country='" & Request.Form("country") & "'"
  sql=sql & " WHERE customerID='" & cid & "'"
  on error resume next
  conn.Execute sql
  if err<>0 then
    response.write("No update permissions!")
  else
    response.write("Record " & cid & " was updated!")
  end if
end if
conn.close
%>

</body>
</html>
```

### 9. Delete Records:
```
<h2>Delete Record</h2>
<%
set conn=Server.CreateObject("ADODB.Connection")
conn.Provider="Microsoft.Jet.OLEDB.4.0"
conn.Open "c:/webdata/northwind.mdb"

cid=Request.Form("customerID")

if Request.form("companyname")="" then
  set rs=Server.CreateObject("ADODB.Recordset")
  rs.open "SELECT * FROM customers WHERE customerID='" & cid & "'",conn
  %>
  <form method="post" action="demo_delete.asp">
  <table>
  <%for each x in rs.Fields%>
  <tr>
  <td><%=x.name%></td>
  <td><input name="<%=x.name%>" value="<%=x.value%>"></td>
  <%next%>
  </tr>
  </table>
  <br><br>
  <input type="submit" value="Delete record">
  </form>
<%
else
  sql="DELETE FROM customers"
  sql=sql & " WHERE customerID='" & cid & "'"
  on error resume next
  conn.Execute sql
  if err<>0 then
    response.write("No update permissions!")
  else
    response.write("Record " & cid & " was deleted!")
  end if
end if
conn.close
%>
```

### 10. ADO Object:

#### 10.1. Command Object
- It is used to execute single query against a database to perform actions like creating, adding, retrieving, deleting or updating records.
- Retrieved data  returned as Recordset object can be manipulated by properties, collections, methods and events of Recordset object.
- its major feature is to use stored queries and procedures with parameters.
```--ProgID
set objCommand=Server.CreateObject("ADODB.command")
```

- Properties:
- a. ActiveConnection: Sets or returns a definition for a connection if the connection is closed, or the current Connection object if the connection is open
- b. CommandText	: Sets or returns a provider command
- c. CommandTimeout: Sets or returns the number of seconds to wait while attempting to execute a command
- d. CommandType: Sets or returns the type of a Command object
- e. Name: Sets or returns the name of a Command object
- f. Prepared	Sets or returns a Boolean value that, if set to True, indicates that the command should save a prepared version of the query before the first execution
- g. State: Returns a value that describes if the Command object is open, closed, connecting, executing or retrieving data
---

- Collections:
- a. Parameters: Contains all the Parameter objects of a Command Object
- b. Properties: Contains all the Property objects of a Command Object
---

- Methods:
- a. Cancel: Cancels an execution of a method
- b. CreateParameter: Creates a new Parameter object
- c. Execute: Executes the query, SQL statement or procedure in the CommandText property

### 10.2. ADO Connection Object
- Used to create an open connection to a data source to access and manipulate database.

- ProgID:
```
set objConnection=Server.CreateObject("ADODB.connection")

```

- Properties:
- a. Attributes: Sets or returns the attributes of a Connection object
- b. CommandTimeout: Sets or returns the number of seconds to wait while attempting to execute a command
- c. ConnectionString: Sets or returns the details used to create a connection to a data source
- f. ConnectionTimeout: Sets or returns the number of seconds to wait for a connection to open
- e. CursorLocation	Sets or returns the location of the cursor service
- f. DefaultDatabase	Sets or returns the default database name
- g. IsolationLevel	Sets or returns the isolation level
- h. Mode	Sets or returns the provider access permission
- i. Provider	Sets or returns the provider name
- j. State	Returns a value describing if the connection is open or closed
- k. Version	Returns the ADO version number
---

- Methods:
- a. BeginTrans: Begins a new transaction
- b. Cancel: Cancels an execution
- c. Close: Closes a connection
- d. CommitTrans: Saves any changes and ends the current transaction
- e. Execute: Executes a query, statement, procedure or provider specific text
- f. Open: Opens a connection
- g. OpenSchema: Returns schema information from the provider about the data source
- h. RollbackTrans: Cancels any changes in the current transaction and ends the transaction
---
- Events:
- a. BeginTransComplete: Triggered after the BeginTrans operation
- b. CommitTransComplete: Triggered after the CommitTrans operation
- c. ConnectComplete: Triggered after a connection starts
- d. Disconnect: Triggered after a connection ends
- e. ExecuteComplete:Triggered after a command has finished executing
- f. InfoMessage: Triggered if a warning occurs during a ConnectionEvent operation
- g. RollbackTransComplete: Triggered after the RollbackTrans operation
- h. WillConnect: Triggered before a connection starts
- i. WillExecute: Triggered before a command is executed
---
- Collections:
- a. Errors: Contains all the Error objects of the Connection object
- b. Properties: Contains all the Property objects of the Connection object

### 10.3. ADO Error Object
- Data access errors generation during a single operation.
- Syntax: ```objErr.property```
- Propertties:
- a. Description: Returns an error description
- b. HelpContext: Returns the context ID of a topic in the Microsoft Windows help system
- c. HelpFile: Returns the full path of the help file in the Microsoft Windows help system
- d. NativeError: Returns an error code from the provider or the data source
- e. Number: Returns a unique number that identifies the error
- f. Source: Returns the name of the object or application that generated the error
- g. SQLState: Returns a 5-character SQL error code

```
for each objErr in objConn.Errors
  response.write("<p>")
  response.write("Description: ")
  response.write(objErr.Description & "<br>")
  response.write("Help context: ")
  response.write(objErr.HelpContext & "<br>")
  response.write("Help file: ")
  response.write(objErr.HelpFile & "<br>")
  response.write("Native error: ")
  response.write(objErr.NativeError & "<br>")
  response.write("Error number: ")
  response.write(objErr.Number & "<br>")
  response.write("Error source: ")
  response.write(objErr.Source & "<br>")
  response.write("SQL state: ")
  response.write(objErr.SQLState & "<br>")
  response.write("</p>")
next
%>
```

### 10.4. ADO Field Object:
- info about a column of Recordset object.
-  ProgID: ``` set objField=Server.CreateObject("ADODB.field")```
---
- Properties: 
- a. ActualSize: Returns the actual length of a field's value
- b. Attributes: Sets or returns the attributes of a Field object
- c. DefinedSize: Returns the defined size of a field
- d. Name: Sets or returns the name of a Field object
- e. NumericScale: Sets or returns the number of decimal places allowed for numeric values in a Field object
- f. OriginalValue: Returns the original value of a field
- g. Precision: Sets or returns the maximum number of digits allowed when representing numeric values in a Field object
- h. Status: Returns the status of a Field object
- i.Type: Sets or returns the type of a Field object
- j. UnderlyingValue: Returns the current value of a field
- k. Value: Sets or returns the value of a Field object
---
- Methods: 
- a. AppendChunk: Appends long binary or character data to a Field object
- b. GetChunk: Returns all or a part of the contents of a large text or binary data Field object


### 10.5. ADO Parameter Object
- Info about a single parameter used in query or stored procedure. (eg. @ID, @Name)
- Used to create a parameterized commands.
- Four types of parameters: input parameters, output parameters, input/output parameters and return parameters.
- Syntax: 
``` 
objectname.property
objectname.method
```
---
- Properties:
- a. Attributes: Sets or returns the attributes of a Parameter object
- b. Direction: Sets or returns how a parameter is passed to or from a procedure
- c. Name: Sets or returns the name of a Parameter object
- d. NumericScale: Sets or returns the number of digits stored to the right side of the decimal point for a numeric value of a Parameter object
- e. Precision: Sets or returns the maximum number of digits allowed when representing numeric values in a Parameter
- f. Size: Sets or returns the maximum size in bytes or characters of a value in a Parameter object
- g. Type: Sets or returns the type of a Parameter object
- h. Value: Sets or returns the value of a Parameter object
---
- Methods:
- a. AppendChunk: Appends long binary or character data to a Parameter object
- b. Delete: Deletes an object from the Parameters Collection


### 10.6. ADO Property Object
- Contains dynamic characteristics of an ADO object defined by provider.
- ProgID: ``` set objProperty=Server.CreateObject("ADODB.property")```


-Properties:
- a. Attributes: Returns the attributes of a Property object
- b. Name: Sets or returns the name of a Property object
- c. Type: Returns the type of a Property object
- d. Value: Sets or returns the value of a Property object

### 10.7. ADO Record Object
- Used to hold a row in a Recordset, a directory or a file from a file system.
- Only structured databases could be accessed by ADO in versions prior 2.5. 
- Syntax: 
```
objectname.property
objectname.method
```

--- 
- Properties:
- a. ActiveConnection: Sets or returns which Connection object a Record object belongs to
- b. Mode: Sets or returns the permission for modifying data in a Record object
- c. ParentURL: Returns the absolute URL of the parent Record
- d. RecordType: Returns the type of a Record object
- e. Source	: Sets or returns the src parameter of the Open method of a Record object
- f. State: Returns the status of a Record object

---
- Methods:
- a.Cancel: Cancels an execution of a CopyRecord, DeleteRecord, MoveRecord, or Open call
- b. Close: Closes a Record object
- c. CopyRecord: Copies a file or directory to another location
- d. DeleteRecord: Deletes a file or directory
- e. GetChildren: Returns a Recordset object where each row represents the files in the directory
- f. MoveRecord: Moves a file or a directory to another location
- g. Open: Opens an existing Record object or creates a new file or directory


### 10.8 ADO Recordset Object
- set of records from a database table, consists of records and columns(fields).
- When you first open a Recordset, the current record pointer will point to the first record and the BOF and EOF properties are False. If there are no records, the BOF and EOF property are True.
- Supports two types of updating: Immediate updating where all changes are written immediately to the database once you call the Update method and Batch updating where provider will cache multiple changes and then send them to the database with the UpdateBatch method.
---
- The cursor type can be set by the CursorType property or by the CursorType parameter in the Open method.
- 4 different cursor types are:
- a. Dynamic cursor - Allows you to see additions, changes, and deletions by other users.
- b. Keyset cursor - Like a dynamic cursor, except that you cannot see additions by other users, and it prevents access to records that other users have deleted. Data changes by other users will still be visible.
- c. Static cursor - Provides a static copy of a recordset for you to use to find data or generate reports. Additions, changes, or deletions by other users will not be visible. This is the only type of cursor allowed when you open a client-side Recordset object.
- d. Forward-only cursor - Allows you to only scroll forward through the Recordset. Additions, changes, or deletions by other users will not be visible. 

---
- Properties:
- AbsolutePage: Sets or returns a value that specifies the page number in the Recordset object
- AbsolutePosition: Sets or returns a value that specifies the ordinal position of the current record in the Recordset object
- ActiveCommand: Returns the Command object associated with the Recordset
- ActiveConnection: Sets or returns a definition for a connection if the connection is closed, or the current Connection object if the connection is open
- BOF: Returns true if the current record position is before the first record, otherwise false
- Bookmark; Sets or returns a bookmark. The bookmark saves the position of the current record
- CacheSize: Sets or returns the number of records that can be cached
- CursorLocation: Sets or returns the location of the cursor service
- CursorType: Sets or returns the cursor type of a Recordset object
- DataMember: Sets or returns the name of the data member that will be retrieved from the object referenced by the DataSource property
- DataSource: Specifies an object containing data to be represented as a Recordset object
- EditMode: Returns the editing status of the current record
- EOF: Returns true if the current record position is after the last record, otherwise false
- Filter: Sets or returns a filter for the data in a Recordset object
- Index: Sets or returns the name of the current index for a Recordset object
- LockType: Sets or returns a value that specifies the type of locking when editing a record in a Recordset
- MarshalOptions: Sets or returns a value that specifies which records are to be returned to the server
- MaxRecords: Sets or returns the maximum number of records to return to a Recordset object from a query
- PageCount: Returns the number of pages with data in a Recordset object
- PageSize: Sets or returns the maximum number of records allowed on a single page of a Recordset object
- RecordCount: Returns the number of records in a Recordset object
- Sort: Sets or returns the field names in the Recordset to sort on
- Source: Sets a string value or a Command object reference, or returns a String value that indicates the data source of the Recordset object
- State: Returns a value that describes if the Recordset object is open, closed, connecting, executing or retrieving data
- Status: Returns the status of the current record with regard to batch updates or other bulk operations
- StayInSync: Sets or returns whether the reference to the child records will change when the parent record position changes

---
- METHODS:
- AddNew: Creates a new record
- Cancel: Cancels an execution
- CancelBatch: Cancels a batch update
- CancelUpdate: Cancels changes made to a record of a Recordset object
- Clone: Creates a duplicate of an existing Recordset
- Close: Closes a Recordset
- CompareBookmarks: Compares two bookmarks
- Delete: Deletes a record or a group of records
- Find: Searches for a record in a Recordset that satisfies a specified criteria
- GetRows: Copies multiple records from a Recordset object into a two-dimensional array
- GetString: Returns a Recordset as a string
- Move: Moves the record pointer in a Recordset object
- MoveFirst: Moves the record pointer to the first record
- MoveLast: Moves the record pointer to the last record
- MoveNext:Moves the record pointer to the next record
- MovePrevious: Moves the record pointer to the previous record
- NextRecordset: Clears the current Recordset object and returns the next Recordset object by looping through a series of commands
- Open: Opens a database element that gives you access to records in a table, the results of a query, or to a saved Recordset
- Requery: Updates the data in a Recordset by re-executing the query that made the original Recordset
- Resync: Refreshes the data in the current Recordset from the original database
- Save: Saves a Recordset object to a file or a Stream object
- Seek: Searches the index of a Recordset to find a record that matches the specified values
- Supports: Returns a boolean value that defines whether or not a Recordset object supports a specific type of functionality
- Update: Saves all changes made to a single record  in a Recordset object
- UpdateBatch: Saves all changes in a Recordset to the database. Used when working in batch update mode

---
- EVENTS:
- EndOfRecordset: Triggered when you try to move to a record after the last record
- FetchComplete: Triggered after all records in an asynchronous operation have been fetched
- FetchProgress; Triggered periodically in an asynchronous operation, to state how many more records that have been fetched
- FieldChangeComplete: Triggered after the value of a Field object change
- MoveComplete: Triggered after the current position in the Recordset has changed
- RecordChangeComplete: Triggered after a record has changed
- RecordsetChangeComplete: Triggered after the Recordset has changed
- WillChangeField: Triggered before the value of a Field object change
- WillChangeRecord: Triggered before a record change
- WillChangeRecordset: Triggered before a Recordset change
- WillMove: Triggered before the current position in the Recordset changes


### 10.9 ADO Stream Object
- Used to read, write and manage stream of binary data or text.
- it can be obtained in three ways: 
- a. rom a URL pointing to a document, a folder, or a Record object
- b. By instantiating a Stream object to store data for your application
- c. By opening the default Stream object associated with a Record object

---
- Properties:
- a. CharSet: Sets or returns a value that specifies into which character set the contents are to be translated. This property is only used with text Stream objects (type is adTypeText)
- b. EOS: Returns whether the current position is at the end of the stream or not
- c. LineSeparator: Sets or returns the line separator character used in a text Stream object
- d. Mode: Sets or returns the available permissions for modifying data
- e. Position: Sets or returns the current position (in bytes) from the beginning of a Stream object
- f. Size: Returns the size of an open Stream object
- g. State: Returns a value describing if the Stream object is open or closed
- h. Type: Sets or returns the type of data in a Stream object

---
- Methods:
- a. Cancel	: Cancels an execution of an Open call on a Stream object
- b. Close: Closes a Stream object
- c. CopyTo: Copies a specified number of characters/bytes from one Stream object into another Stream object
- d. Flush: Sends the contents of the Stream buffer to the associated underlying object
- e. LoadFromFile: Loads the contents of a file into a Stream object
- f. Open: Opens a Stream object
- g. Read: Reads the entire stream or a specified number of bytes from a binary Stream object
- h. ReadText: Reads the entire stream, a line, or a specified number of characters from a text Stream object
- i. SaveToFile: Saves the binary contents of a Stream object to a file
- j. SetEOS: Sets the current position to be the end of the stream (EOS)
- k. SkipLine: Skips a line when reading a text Stream
- l. Write: Writes binary data to a binary Stream object
- m. WriteText: Writes character data to a text Stream object

### 10.10. ADO Data Types
- In Access, there are YesNo, Currency, Date, DateTime, Double, AutoNumber Integer Long, Single, Integer, Byte, text.
- In SQL Server, Bit, Char, BigIng, Binary timestamp, money smallmoney, Datetime, Float, identity Int, Image, Int, Decimal Numeric, smallInt, tinytin, VarBinary, VarChar.





