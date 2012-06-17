(function() {
  var createRuleItem, loadRuleItems;

  $(function() {
    var _this = this;
    $.post('RuleEditor/getRuleItems', $('#ruleName').val(), loadRuleItems, "json");
    return $('li', '.rule-item-list').mousedown(function(event) {
      var item;
      item = createRuleItem($(_this).data(), event.pageX, event.pageY);
      $(".rule-editor").append(item);
      return item.trigger('mousedown');
    });
  });

  loadRuleItems = function(data) {
    var htmlItem, ruleItem, _i, _len, _ref, _results;
    _ref = $.JSON(data);
    _results = [];
    for (_i = 0, _len = _ref.length; _i < _len; _i++) {
      ruleItem = _ref[_i];
      htmlItem = createRuleItem(ruleItem, ruleItem.position.X, ruleItem.position.Y);
      _results.push($(".rule-editor").append(htmlItem));
    }
    return _results;
  };

  createRuleItem = function(ruleItem, posX, posY) {
    var htmlCaption, htmlImg, htmlItem,
      _this = this;
    htmlImg = $("<div class='rule-item-img' />").append("<img src='" + ruleItem.backgroundImg + "'>");
    htmlCaption = $("<span />").text(ruleItem.caption);
    htmlItem = $("<div class='rule-item' />").append(htmlImg).append(htmlCaption);
    htmlItem.css('left', posX).css('top', posY);
    htmlItem.click(function(event) {
      return showMenu(_this);
    });
    return htmlItem.data(ruleItem).draggable();
  };

}).call(this);
