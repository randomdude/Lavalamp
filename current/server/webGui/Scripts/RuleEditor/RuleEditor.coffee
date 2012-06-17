$ ->
	$.post( 
		'RuleEditor/getRuleItems',
		$('#ruleName').val(),
		loadRuleItems,
		"json"
	)
	# setup the rule-item-list to allow the user to add new rule items to the rule
	$('li','.rule-item-list').mousedown (event) => 
		item = createRuleItem($(this).data(), event.pageX, event.pageY)
		$(".rule-editor").append(item)
		item.trigger('mousedown')

loadRuleItems = (data) ->
	for ruleItem in $.JSON(data)
		htmlItem = createRuleItem(ruleItem, ruleItem.position.X, ruleItem.position.Y)
		$(".rule-editor").append(htmlItem)

createRuleItem = (ruleItem, posX, posY) ->
	htmlImg = $("<div class='rule-item-img' />").append("<img src='#{ruleItem.backgroundImg}'>")
	htmlCaption = $("<span />").text(ruleItem.caption)
	htmlItem = $("<div class='rule-item' />").append(htmlImg).append(htmlCaption)
	htmlItem.css('left',posX).css('top',posY)
	htmlItem.contextMenu({menu: '#context'})
	htmlItem.data(ruleItem).draggable()