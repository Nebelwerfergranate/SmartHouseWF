<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SmartHouseWF.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="Content/jquery-ui.min.css" />
    <link rel="stylesheet" href="Content/jquery.jgrowl.css" />
    <link rel="stylesheet" href="Content/jquery.myClock.css" />
    <link rel="stylesheet" href="Content/css.css" />
    <script src="scripts/jquery-2.1.4.min.js"></script>
    <script src="scripts/jquery-ui.min.js"></script>
    <script src="scripts/jquery.color.js"></script>
    <script src="scripts/jquery.jgrowl.js"></script>
    <script src="scripts/jquery.myClock.js"></script>
    <script src="scripts/script.js"></script>
</head>
<body>
    <form id="form" runat="server">
        <div>
            <div hidden="hidden">
                <input runat="server" id="DeviceName" type="text" class="js_DeviceName" />
            </div>
            <div class="add-device-panel">
                <div>
                    <div class="image">
                        <img src="Content/Images/clock.png" />
                    </div>
                    <div class="add-device-control">
                        <asp:Button runat="server" ID="AddclockButton" Text="Add" OnClick="AddclockButton_OnClick" CssClass="js_AddButton, single-add" />
                    </div>
                </div>
                <div>
                    <div class="image">
                        <img src="Content/Images/microwave.png" />
                    </div>
                    <div class="add-device-control">
                        <asp:DropDownList runat="server" ID="AddMicrowaveList" />
                        <asp:Button runat="server" ID="AddMicrowaveButton" Text="Add" OnClick="AddMicrowaveButton_OnClick" CssClass="js_AddButton" />
                    </div>
                </div>
                <div>
                    <div class="image">
                        <img src="Content/Images/oven.png" />
                    </div>
                    <div class="add-device-control">
                        <asp:DropDownList runat="server" ID="AddOvenList" />
                        <asp:Button runat="server" ID="AddOvenButton" Text="Add" OnClick="AddOvenButton_OnClick" CssClass="js_AddButton" />
                    </div>
                </div>
                <div>
                    <div class="image">
                        <img src="Content/Images/fridge.png" />
                    </div>
                    <div class="add-device-control">
                        <asp:DropDownList runat="server" ID="AddFridgeList" />
                        <asp:Button runat="server" ID="AddFridgeButton" Text="Add" OnClick="AddFridgeButton_OnClick" CssClass="js_AddButton" />
                    </div>
                </div>
            </div>

            <asp:Repeater ID="Repeater1" OnItemCommand="OnItemCommand" OnItemDataBound="OnItemDataBound" runat="server">
                <ItemTemplate>
                    <asp:Panel ID="DevicePanel" runat="server" CssClass="device">
                        <!-- Device -->
                        <asp:Button runat="server" CommandName="Remove" Text="Remove"></asp:Button>
                        <asp:Button runat="server" CommandName="Toggle" Text="Toggle"></asp:Button>
                        <asp:Button runat="server" CommandName="Rename" Text="Rename" CssClass="js_RenameButton" />
                        <asp:Label runat="server" ID="Name"></asp:Label>
                        <asp:Label runat="server" ID="State"></asp:Label>
                        <asp:HiddenField runat="server" ID="DeviceID" />
                        <!-- IBacklight -->
                        <asp:Panel runat="server" ID="IBacklightPanel" Visible="False">
                            <asp:Label runat="server" ID="IsHighlightedLabel"></asp:Label>
                        </asp:Panel>
                        <!-- IClock -->
                        <asp:Panel runat="server" ID="CurrentTime" Visible="False" CssClass="js_IClockDiv">
                            <asp:HiddenField runat="server" ID="HiddenTimestamp" />
                            <div class="js_dynamicClockDiv">This div will be turned into a dynamic clock</div>
                            <asp:TextBox runat="server" ID="Hours" MaxLength="2" Visible="False" CssClass="js_HoursSetField"></asp:TextBox>
                            <asp:Label runat="server" ID="ClockTimeSeparator" Visible="False">:</asp:Label>
                            <asp:TextBox runat="server" ID="Minutes" MaxLength="2" Visible="False" CssClass="js_MinutesSetField"></asp:TextBox>
                            <asp:Button runat="server" ID="SetTimeButton" Text="Set Time" CommandName="SetTime" Visible="False" CssClass="js_TimeSetSubmit" />
                        </asp:Panel>
                        <!-- IOpenable -->
                        <asp:Panel runat="server" ID="IOpenablePanel" Visible="False">
                            <asp:Label runat="server" ID="IsOpenLabel"></asp:Label>
                            <asp:Button runat="server" ID="OpenButton" CommandName="Open" Text="Open" />
                            <asp:Button runat="server" ID="CloseButton" CommandName="Close" Text="Close" />
                        </asp:Panel>
                        <!-- ITemperature -->
                        <asp:Panel runat="server" ID="ITemperaturePanel" Visible="False" CssClass="js_ITemperatureDiv">
                            <asp:TextBox runat="server" ID="TemperatureTextBox" CssClass="js_TemperatureSetField" MaxLength="5"></asp:TextBox>
                            <asp:Button runat="server" ID="SetTemperatureButton" CommandName="SetTemperature" Text="Set temperature" CssClass="js_TemperatureSetSubmit" />
                            <span hidden="hidden" class="js_MinTemperatureSpan">
                                <asp:HiddenField runat="server" ID="MinTemperature" />
                            </span>
                            <span hidden="hidden" class="js_MaxTemperatureSpan">
                                <asp:HiddenField runat="server" ID="MaxTemperature" />
                            </span>
                        </asp:Panel>
                        <!-- ITimer -->
                        <asp:Panel runat="server" ID="ITimerPanel" Visible="False" CssClass="js_ITimerDiv">
                            <asp:Label runat="server" ID="TimerIsRunning"></asp:Label>
                            <asp:Panel runat="server" ID="ITimerDynamicPanel" Visible="False">
                                <asp:Button runat="server" ID="StartButton" Text="Start" CommandName="StartTimer" />
                                <asp:Button runat="server" ID="StopButton" Text="Stop" CommandName="StopTimer" />
                                <asp:TextBox runat="server" ID="TimerHours" MaxLength="2" CssClass="js_HoursSetField"></asp:TextBox>
                                <span runat="server" id="TimerHoursSeparator">:</span>
                                <asp:TextBox runat="server" ID="TimerMinutes" MaxLength="2" CssClass="js_MinutesSetField"></asp:TextBox>
                                <span>:</span>
                                <asp:TextBox runat="server" ID="TimerSeconds" MaxLength="2" CssClass="js_SecondsSetField"></asp:TextBox>
                                <asp:Button runat="server" ID="SetTimerButton" Text="Set Timer" CommandName="SetTimer" CssClass="js_TimeSetSubmit" />
                            </asp:Panel>
                        </asp:Panel>
                        <!-- IVolume -->
                        <asp:Panel runat="server" ID="IVolumePanel" Visible="False">
                            <asp:Label runat="server" ID="VolumeLabel"></asp:Label>
                        </asp:Panel>
                        <!-- Fridge -->
                        <asp:Panel runat="server" ID="FridgePanel" Visible="False">
                            <!-- Coldstore -->
                            <div>
                                <asp:Label runat="server" ID="ColdstoreIsOpenLabel"></asp:Label>
                                <asp:Button runat="server" ID="ColdstoreOpenButton" CommandName="OpenColdstore" Text="Open coldstore" />
                                <asp:Button runat="server" ID="ColdstoreCloseButton" CommandName="CloseColdstore" Text="Close coldstore" />

                                <div class="js_ITemperatureDiv">
                                    <asp:TextBox runat="server" ID="ColdstoreTemperatureTextBox" CssClass="js_TemperatureSetField" MaxLength="5"></asp:TextBox>
                                    <asp:Button runat="server" ID="ColdstoreSetTemperatureButton" CommandName="ColdstoreSetTemperature" Text="Set coldstore temperature" CssClass="js_TemperatureSetSubmit" />
                                    <span hidden="hidden" class="js_MinTemperatureSpan">
                                        <asp:HiddenField runat="server" ID="ColdstoreMinTemperature" />
                                    </span>
                                    <span hidden="hidden" class="js_MaxTemperatureSpan">
                                        <asp:HiddenField runat="server" ID="ColdstoreMaxTemperature" />
                                    </span>
                                </div>

                                <asp:Label runat="server" ID="ColdstoreIsHighlightedLabel"></asp:Label>
                                <asp:Label runat="server" ID="ColdstoreVolumeLabel"></asp:Label>
                            </div>
                            <!-- Freezer -->
                            <div>
                                <asp:Label runat="server" ID="FreezerIsOpenLabel"></asp:Label>
                                <asp:Button runat="server" ID="FreezerOpenButton" CommandName="OpenFreezer" Text="Open Freezer" />
                                <asp:Button runat="server" ID="FreezerCloseButton" CommandName="CloseFreezer" Text="Close Freezer" />

                                <div class="js_ITemperatureDiv">
                                    <asp:TextBox runat="server" ID="FreezerTemperatureTextBox" CssClass="js_TemperatureSetField" MaxLength="5"></asp:TextBox>
                                    <asp:Button runat="server" ID="FreezerSetTemperatureButton" CommandName="FreezerSetTemperature" Text="Set Freezer temperature" CssClass="js_TemperatureSetSubmit" />
                                    <span hidden="hidden" class="js_MinTemperatureSpan">
                                        <asp:HiddenField runat="server" ID="FreezerMinTemperature" />
                                    </span>
                                    <span hidden="hidden" class="js_MaxTemperatureSpan">
                                        <asp:HiddenField runat="server" ID="FreezerMaxTemperature" />
                                    </span>
                                </div>

                                <asp:Label runat="server" ID="FreezerVolumeLabel"></asp:Label>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </form>
</body>
</html>
