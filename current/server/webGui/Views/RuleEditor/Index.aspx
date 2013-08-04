<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<webGui.Controllers.Models.ruleEditorModel>" %>
<%@ Import Namespace="ruleEngine" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <% if (Model.newRule)
       {%>
       New rule
        <% }
       else
       { %>
    Editing rule, <%= Model.currentRule.name %>
    <% } %>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ScriptContent">
    <script type="text/javascript">
        preferredHeight = <%= Model.currentRule.preferredHeight%>;
        preferredWidth = <%= Model.currentRule.preferredWidth%>;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% if (Model.newRule) {%>
        <%= Html.LabelFor(r => r.currentRule.name) %><%= Html.TextBoxFor(r => r.currentRule.name, new { required = "required", placeholder= "rule name"})%> <%= Ajax.ActionLink("Create", "saveRule", new AjaxOptions() { HttpMethod = "POST", OnComplete = "NewRuleCreated" })%>
    <% } else { %>
    <input type="hidden" id="ruleName" value="<%= Model.currentRule.name %>"/>
    <% } %>
    <menu id="rule-item-context" type="context">
        <menuitem id="context-delete">Delete</menuitem>
        <menuitem type="command" label="Options" id="context-options">Options</menuitem>
    </menu>
    <div id="rule-editor" style='<%= Model.newRule ? "display='none'" : "" %>' >
        <canvas id="draw-area">
        </canvas>
        <div id="rule-item-area">
        </div>
    </div>
   
    <% if (!Model.newRule)
       {%>
    <input type="button" value="<%= Model.currentRule.state == ruleEngine.ruleState.running ? "Stop Rule" : "Run Rule" %>" />
    <% } %>
     <div class="rule-item-list">
        <ul>      
            <%= Model.ruleItemTree() %>
       </ul>
    </div>
    
    <div id="options-dialog">
        <% using (Ajax.BeginForm(new AjaxOptions{}))
           {%>
        <% } %>
    </div>

</asp:Content>
