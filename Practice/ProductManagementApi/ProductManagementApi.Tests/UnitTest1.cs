namespace ProductManagementApi.Tests;

public class UnitTest1
{
    [Fact]  //tells xUnit that this method is a test to be discovered and executed.
    public void Add_TwoNumbers_ReturnsCorrectResult()
    {
        //Implement AAA Pattern
        //Arrange (Prepare objects and data needed for test)
        var calculator = new Calculator();

        //Act (Execute method to test)
        var result = calculator.Add(10, 20);

        //Assert (Verify actual result as expected result)
        Assert.Equal(30, result);

    }
}

//putting here class class for test
public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
}
