<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Import Namespace="System.Activities.Statements" %>
<%@ Import Namespace="System.ComponentModel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <asp:Panel ID="Panel1" runat="server">
            </asp:Panel>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="1" />
            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="2" />
            <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="3" />

            <br />
             <asp:Label runat="server" ID="Label1"></asp:Label>
        </div>
        <asp:Repeater ID="Repeater1" runat="server" OnItemCreated="Repeater1_ItemCreated">
            <ItemTemplate>
                
               <% Label1.Text = currentItem.ToString(); %>
               
                <% if (currentItem.ToString() == "1")
                         { %>
                <input type="radio"></input>
                <% }
                         else if (currentItem.ToString() == "2")
                         { %>
                <input type="checkbox"/>
                <% }
                         else if (currentItem.ToString() == "3")
                         { %>
                <input type="text"/>
                <% } %>
                <br />
            </ItemTemplate>
        </asp:Repeater>
    </form>
</body>
</html>
