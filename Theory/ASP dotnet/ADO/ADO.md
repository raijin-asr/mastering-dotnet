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



