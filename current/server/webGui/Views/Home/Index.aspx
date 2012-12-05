<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<webGui.Models.ruleOverviewModel>" %>
<%@ Import Namespace="lavalamp" %>
<%@ Import Namespace="ruleEngine" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Summary:</h2>
    <ul>
        <li>Number of active rules: <%=Model.Count(r => r.state == ruleState.running) %></li>
        <li>Number of distinct boxes: </li>
        <li>Number of users: </li>
        <li>Uptime: </li>
    </ul>  
    <% using (Html.BeginForm("Index", "RuleEditor", FormMethod.Post)) { %>
    <table class="centre">
        <thead>
            <tr>
                <th>Rule State</th>
                <th>Name</th>
                <th>Location</th>
                <th>Details</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
       
    <% foreach (lavalampRuleInfo rule in Model)
       {%>
         <tr>
             <td class="<%=Model.getStateClass(rule)%>"><%=rule.state.HasValue ? rule.state.ToString() : "Unknown" %></td>
             <td><a href="noJS.html" class="post-link" ><%= rule.name %></a></td>
             <td><%= rule.details %></td>
             <td></td>
             <td><% if (rule.state != ruleState.errored)
                    {%>
             <%= Html.ActionLink(rule.state == ruleState.stopped ? "Run" : "Stop", "ChangeRuleState", rule) %>
            <% } %>
               <%= Html.ActionLink("Delete", "deleteRule", rule) %>
            </td>
            
         </tr>   
       <% } %>
       </tbody>
    </table>
    <% } %>
    <details>
        <summary>
        
        </summary>
    </details>
</asp:Content>
