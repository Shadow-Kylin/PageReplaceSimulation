<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Simulation.aspx.cs" Inherits="PageReplaceSimulation.Simulation" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%--页面设计--%>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>页面置换算法模拟</title>
    <link rel="stylesheet" href="StyleSheet1.css" />
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <div>
            <div class="opui">
                <div class="para1">
                    <asp:Label ID="Label1" runat="server" Text="Label">最大内存页数</asp:Label>
                    <asp:DropDownList ID="DropDownList1" runat="server">
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="para2">
                    <asp:Label ID="Label2" runat="server" Text="Label">产生的随机页面最大页号</asp:Label>
                    <asp:DropDownList ID="DropDownList2" runat="server">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>25</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="para3">
                    <asp:Label ID="Label3" runat="server" Text="Label">页面数</asp:Label>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </div>
                <asp:Button ID="Button1" runat="server" Text="开始模拟" OnClick="Button1_Click" />
            </div>
            <div style="position:absolute;top:15%">❌：缺页，✔️：不缺页</div>
            <div id="show" class="show" runat="server">
                <details open="open">
                    <summary>结果</summary>
                    <fieldset class="fieldset1">
                        <legend>FIFO先进先出</legend>
                        <asp:Label ID="Label4" runat="server"></asp:Label>
                    </fieldset>
                    <fieldset class="fieldset2">
                        <legend>OPT理想型淘汰</legend>
                        <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
                    </fieldset>
                    <fieldset class="fieldset3">
                        <legend>LRU最近最久未使用</legend>
                        <asp:Label ID="Label6" runat="server" Text="Label"></asp:Label>
                    </fieldset>
                </details>
                <details open="open">
                    <summary>缺页率比较</summary>
                    <fieldset class="compare">
                        <asp:Label ID="Label7" runat="server" Text="Label"></asp:Label>
                        <asp:Chart ID="Chart1" runat="server" Width="700px">
                            <Series>
                                <asp:Series Name="Series1" LegendText="FIFO" Legend="FIFO" IsValueShownAsLabel="True" ChartArea="ChartArea1" ShadowColor="#336699" ShadowOffset="3" ChartType="Bar" YValuesPerPoint="4"></asp:Series>
                                <asp:Series Name="Series2" LegendText="OPT" Legend="OPT" IsValueShownAsLabel="True" ChartArea="ChartArea1" ShadowColor="#336699" ShadowOffset="3" ChartType="Bar" YValuesPerPoint="4"></asp:Series>
                                <asp:Series Name="Series3" LegendText="LRU" Legend="LRU" IsValueShownAsLabel="True" ChartArea="ChartArea1" ShadowColor="#336699" ShadowOffset="3" ChartType="Bar" YValuesPerPoint="4"></asp:Series>
                            </Series>
                            <Legends>
                                <asp:Legend Name="FIFO">
                                </asp:Legend>
                                <asp:Legend Name="OPT">
                                    <Position Width="15" Height="15" X="83" Y="20" />
                                </asp:Legend>
                                <asp:Legend Name="LRU">
                                    <Position Width="15" Height="15" X="83" Y="40" />
                                </asp:Legend>
                            </Legends>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                    <AxisX Title="内存容量"></AxisX>
                                    <AxisY Title="缺页率"></AxisY>
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                    </fieldset>
                </details>
            </div>
        </div>
    </form>
</body>
</html>
