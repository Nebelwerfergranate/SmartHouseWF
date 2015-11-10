<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SmartHouseWF.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Panel ID="Panel1" runat="server">
                <input type="text" name="newDeviceName" value=""/>
                <asp:RadioButton runat="server" ID="ClockRadio" GroupName="DeviceType" Text="Часы" Checked="True"/>
                <asp:RadioButton runat="server" ID="SomethingElseRadio" GroupName="DeviceType" Text="Что-то еще" />
                <asp:Button ID="AddButton" runat="server" OnClick="AddButton_Click" Text="Добавить устройство" />
                <br />
                <asp:Label ID="Messanger" runat="server" Text="всё ок"></asp:Label>
            </asp:Panel>


            <asp:Repeater ID="Repeater1" OnItemCommand="OnItemCommand" OnItemDataBound="OnItemDataBound" runat="server">
                <ItemTemplate>
                    <div>
                        <asp:Panel ID="DevicePanel" runat="server">
                            <!-- Device -->
                            <asp:Button runat="server" CommandName="Remove" Text="Remove"></asp:Button>
                            <asp:Button runat="server" CommandName="Toggle" Text="Toggle"></asp:Button>&nbsp;
                            <br />
                            <asp:Label runat="server" ID="Name"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="State"></asp:Label>
                            <br />
                            <!-- IClock -->
                            <asp:Label runat="server" ID="CurrentTime" Visible="False"></asp:Label>

                        </asp:Panel>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </form>
</body>
</html>
