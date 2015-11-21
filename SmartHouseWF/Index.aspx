<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SmartHouseWF.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="Content/jquery-ui.min.css"/>
    <link rel="stylesheet" href="Content/jquery.jgrowl.css"/>
    <link rel="stylesheet" href="Content/jquery.myClock.css" />
    <link rel="stylesheet" href="Content/css.css"/>
    <script src="scripts/jquery-2.1.4.min.js"></script>
    <script src="scripts/jquery-ui.min.js"></script>
    <script src="scripts/jquery.color.js"></script>
    <script src="scripts/jquery.jgrowl.js"></script>
    <script src="scripts/jquery.myClock.js"></script>
    <script src="scripts/script.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Panel ID="Panel1" runat="server">
                <input type="text" name="newDeviceName" value="" />
                <asp:RadioButton runat="server" ID="ClockRadio" GroupName="DeviceType" Text="Часы" Checked="True" />
                <asp:RadioButton runat="server" ID="Oven" GroupName="DeviceType" Text="Духовка"/>
                <asp:RadioButton runat="server" ID="SomethingElseRadio" GroupName="DeviceType" Text="Что-то еще" />
                <asp:Button ID="AddButton" runat="server" OnClick="AddButton_Click" Text="Добавить устройство" />
                <asp:Label ID="Messanger" runat="server" Text="всё ок"></asp:Label>
            </asp:Panel>


            <asp:Repeater ID="Repeater1" OnItemCommand="OnItemCommand" OnItemDataBound="OnItemDataBound" runat="server">
                <ItemTemplate>
                        <asp:Panel ID="DevicePanel" runat="server" CssClass="device">
                            <!-- Device -->
                            <asp:Button runat="server" CommandName="Remove" Text="Remove"></asp:Button>
                            <asp:Button runat="server" CommandName="Toggle" Text="Toggle"></asp:Button>
                            <asp:Label runat="server" ID="Name" CssClass="js_DeviceName"></asp:Label>
                            <asp:Label runat="server" ID="State"></asp:Label>
                            <asp:HiddenField runat="server" ID="DeviceID"/>
                            <!-- IClock -->
                            <asp:Panel runat="server" ID="CurrentTime" Visible="False" CssClass="js_TimeDiv">
                                <asp:HiddenField runat="server"  ID="js_currentTime"/>
                                <div class="js_timeField">This div will be turned into a dynamic clock</div>
                                <asp:TextBox runat="server" ID="Hours" MaxLength="2" Visible="False" CssClass="js_HoursSetField"></asp:TextBox>
                                <asp:Label runat="server" ID="TimeSeparator" Visible="False">:</asp:Label>
                                <asp:TextBox runat="server" ID="Minutes" MaxLength="2" Visible="False" CssClass="js_MinutsSetField"></asp:TextBox>
                                <asp:Button runat="server" ID="SetTimeButton" Text="Set Time" CommandName="SetTime" Visible="False" CssClass="js_TimeSetSubmit"/>
                            </asp:Panel>
                            <!-- ITimer -->
                            <asp:Panel runat="server" ID="ITimerPanel" Visible="False" CssClass="js_TimerDiv">
                                <asp:Label runat="server" ID="TimerIsRunning" Visible="False"></asp:Label>
                                <asp:Button runat="server" ID="StartButton" Text="Start" CommandName="StartTimer" Visible="False"/>
                                <asp:Button runat="server" ID="StopButton" Text="Stop" CommandName="StopTimer" Visible="False"/>

                                <asp:TextBox runat="server" ID="TimerMinutes" MaxLength="2" Visible="False" CssClass="js_MinutsSetField"></asp:TextBox>
                                <asp:Label runat="server" ID="TimerTimeSeparator" Visible="False">:</asp:Label>
                                <asp:TextBox runat="server" ID="TimerSeconds" MaxLength="2" Visible="False" CssClass="js_SecondsSetField"></asp:TextBox>
                                <asp:Button runat="server" ID="SetTimerButton" Text="Set Time" CommandName="SetTimer" Visible="False" CssClass="js_TimeSetSubmit"/>
                               
                                <input type="button" value="Тест печки" class="js_TimerButton"/>
                            </asp:Panel>
                        </asp:Panel>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </form>
</body>
</html>
