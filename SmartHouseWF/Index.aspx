﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SmartHouseWF.WebForm1" %>

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
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        
        <asp:Repeater ID="Repeater1" runat="server">
            
        </asp:Repeater>
    </div>
    </form>
</body>
</html>