ratioX = 0
ratioY = 0
$ ->
    ruleEditor = $("#rule-editor")
    ratioX = translationRatio(preferredWidth, 0, ruleEditor.width(), 0)
    ratioY = translationRatio(preferredHeight, 0, ruleEditor.height(), 0)
    await $.getJSON "RuleEditor/getRuleItems?selectedRule=#{$('#ruleName').val()}", defer ruleItems
    renderedItems = for ruleItem in ruleItems
        ruleItem.position.x = Math.abs(ruleItem.position.x / ruleEditor.width()) * ruleEditor.width()
        ruleItem.position.y = Math.abs(ruleItem.position.y / ruleEditor.height()) * ruleEditor.height()
        createRuleItem(ruleItem, ruleItem.position.x, ruleItem.position.y)
    for ruleItem in ruleItems
        for guid, p of ruleItem.pins when p.pin.connected isnt '00000000-0000-0000-0000-000000000000' and p.pin.direction isnt 'output'
            drawLine $("##{p.pinid}"),$("##{p.pin.connected}")

    $.contextMenu('html5')
    $('#context-delete').click((e) ->
        $(this).parent().data('ruleItemSelected').remove()
        $(this).parent().data('ruleItemSelected', null)
    )
    $('#context-options').click(() ->
        ruleItem = $(this).parent().data('ruleItemSelected')
        dialogFactory.getOptionForm(ruleItem.opts)
        $(this).parent().data('ruleItemSelected', null)
    )
    # setup the rule-item-list to allow the user to add new rule items to the rule
    $('.rule-item-list').jstree({ "themes": { "theme": "default", "dots": true, "icons": false },"plugins": ["themes", "html_data"] })
    .bind('mousedown.jstree', (event, data) ->
        if (!$(event.target).hasClass('rule-item'))
            return
        item = createRuleItem($(this).data(), event.pageX, event.pageY)
        
        item.trigger(event)
        )    

translationRatio = (srcMax,srcMin,destMax,destMin) -> Math.abs(srcMax-srcMin) / Math.abs(destMax-destMin)

createRuleItem = (ruleItem, posX, posY) ->
    htmlImg = $("<div class='rule-item-img' />").append("<img src='#{ruleItem.backgroundImg}'>")
    htmlCaption = $("<span />").text(ruleItem.caption)
    htmlItem = $("<div class='rule-item' />").append(htmlImg).append(htmlCaption)
    htmlItem.attr('contextmenu','rule-item-context')
    htmlItem.attr('contextmenu', 'rule-item-context').bind('contextmenu', (e) -> 
        $('#' + $(this).attr('contextmenu')).data('ruleItemSelected', $(this) ) )
    #context = $('#draw-area')[0].getContext('2d')
    #context.fillStyle="#ff0000";   
    #context.fillRect(posX, posY, ruleItem.height,ruleItem.width)
    htmlItem.css('left', posX )
    htmlItem.css('top', posY )
    htmlItem.css('height',ruleItem.height)
    htmlItem.css('width',ruleItem.width)
    htmlItem.data(ruleItem).draggable()
    for index, p of ruleItem.pins
        pin = p.pin
        htmlItem.append $("<div id='#{p.pinid}' class='pin #{pin.direction}' title='#{pin.description}'>")
    $("#rule-item-area").append(htmlItem)
    return htmlItem

drawLine = (from, to) ->
    context = $('#draw-area')[0].getContext('2d')
    context.beginPath()
    x = from.parent().position().left 
    y = from.parent().position().top + from.position().top
    context.moveTo(x,y)
    x = to.parent().position().left + to.parent().width()
    y = to.parent().position().top + to.position().top
    context.lineTo(x,y)
    context.lineWidth = 1
    context.strokeStyle = "#ff0000"
    context.closePath()
    context.stroke()