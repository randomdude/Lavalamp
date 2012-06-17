<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<webGui.Controllers.Models.ruleEditorModel>" %>
<%@ Import Namespace="ruleEngine" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2></h2>
    <% if (Model.newRule) {%>
        <%= Html.LabelFor(r => r.currentRule.name) %><%= Html.TextBoxFor(r => r.currentRule.name)%> <%= Ajax.ActionLink("Create", "saveRule", new AjaxOptions() { HttpMethod = "POST", OnComplete = "NewRuleCreated" })%>
    <% } %>
    <div class="rule-editor" style='<%= Model.newRule ? "display='none'" : "" %>' />
    <div class="rule-item-list">
        <ul>
        <% foreach (IRuleItem ruleItem in Model.ruleItems)
           { %>
              <li><%= ruleItem.ruleName() %></li>
        <% } %>
       </ul>
    </div>
    <% if (!Model.newRule)
       {%>
    <input type="button" value="<%= Model.currentRule.state == ruleEngine.ruleState.running ? "Stop Rule" : "Run Rule" %>" />
    <% } %>
</asp:Content>
