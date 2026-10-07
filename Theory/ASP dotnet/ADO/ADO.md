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





