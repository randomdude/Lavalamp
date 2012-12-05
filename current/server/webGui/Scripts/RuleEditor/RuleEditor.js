(function() {
  var createRuleItem, drawLine, iced, ratioX, ratioY, translationRatio, __iced_k, __iced_k_noop,
    __slice = [].slice;

  iced = {
    Deferrals: (function() {

      function _Class(_arg) {
        this.continuation = _arg;
        this.count = 1;
        this.ret = null;
      }

      _Class.prototype._fulfill = function() {
        if (!--this.count) return this.continuation(this.ret);
      };

      _Class.prototype.defer = function(defer_params) {
        var _this = this;
        ++this.count;
        return function() {
          var inner_params, _ref;
          inner_params = 1 <= arguments.length ? __slice.call(arguments, 0) : [];
          if (defer_params != null) {
            if ((_ref = defer_params.assign_fn) != null) {
              _ref.apply(null, inner_params);
            }
          }
          return _this._fulfill();
        };
      };

      return _Class;

    })(),
    findDeferral: function() {
      return null;
    }
  };
  __iced_k = __iced_k_noop = function() {};

  ratioX = 0;

  ratioY = 0;

  $(function() {
    var guid, p, renderedItems, ruleEditor, ruleItem, ruleItems, ___iced_passed_deferral, __iced_deferrals, __iced_k,
      _this = this;
    __iced_k = __iced_k_noop;
    ___iced_passed_deferral = iced.findDeferral(arguments);
    ruleEditor = $("#rule-editor");
    ratioX = translationRatio(preferredWidth, 0, ruleEditor.width(), 0);
    ratioY = translationRatio(preferredHeight, 0, ruleEditor.height(), 0);
    (function(__iced_k) {
      __iced_deferrals = new iced.Deferrals(__iced_k, {
        parent: ___iced_passed_deferral
      });
      $.getJSON("RuleEditor/getRuleItems?selectedRule=" + ($('#ruleName').val()), __iced_deferrals.defer({
        assign_fn: (function() {
          return function() {
            return ruleItems = arguments[0];
          };
        })(),
        lineno: 7
      }));
      __iced_deferrals._fulfill();
    })(function() {
      var _i, _len, _ref;
      renderedItems = (function() {
        var _i, _len, _results;
        _results = [];
        for (_i = 0, _len = ruleItems.length; _i < _len; _i++) {
          ruleItem = ruleItems[_i];
          ruleItem.position.x *= ratioX;
          ruleItem.position.y *= ratioY;
          _results.push(createRuleItem(ruleItem, ruleItem.position.x, ruleItem.position.y));
        }
        return _results;
      })();
      for (_i = 0, _len = ruleItems.length; _i < _len; _i++) {
        ruleItem = ruleItems[_i];
        _ref = ruleItem.pins;
        for (guid in _ref) {
          p = _ref[guid];
          if (p.pin.connected !== '00000000-0000-0000-0000-000000000000' && p.pin.direction !== 'output') {
            drawLine($("#" + p.pinid), $("#" + p.pin.connected));
          }
        }
      }
      $.contextMenu('html5');
      $('#context-delete').click(function(e) {
        $(this).parent().data('ruleItemSelected').remove();
        return $(this).parent().data('ruleItemSelected', null);
      });
      $('#context-options').click(function() {
        ruleItem = $(this).parent().data('ruleItemSelected');
        dialogFactory.getOptionForm(ruleItem.opts);
        return $(this).parent().data('ruleItemSelected', null);
      });
      return $('.rule-item-list').jstree({
        "themes": {
          "theme": "default",
          "dots": true,
          "icons": false
        },
        "plugins": ["themes", "html_data"]
      }).bind('mousedown.jstree', function(event, data) {
        var item;
        if (!$(event.target).hasClass('rule-item')) return;
        item = createRuleItem($(this).data(), event.pageX, event.pageY);
        return item.trigger(event);
      });
    });
  });

  translationRatio = function(srcMax, srcMin, destMax, destMin) {
    return Math.abs(srcMax - srcMin) / Math.abs(destMax - destMin);
  };

  createRuleItem = function(ruleItem, posX, posY) {
    var htmlCaption, htmlImg, htmlItem, index, p, pin, _ref;
    htmlImg = $("<div class='rule-item-img' />").append("<img src='" + ruleItem.backgroundImg + "'>");
    htmlCaption = $("<span />").text(ruleItem.caption);
    htmlItem = $("<div class='rule-item' />").append(htmlImg).append(htmlCaption);
    htmlItem.attr('contextmenu', 'rule-item-context');
    htmlItem.attr('contextmenu', 'rule-item-context').bind('contextmenu', function(e) {
      return $('#' + $(this).attr('contextmenu')).data('ruleItemSelected', $(this));
    });
    htmlItem.css('left', posX);
    htmlItem.css('top', posY);
    htmlItem.css('height', ruleItem.height);
    htmlItem.css('width', ruleItem.width);
    htmlItem.data(ruleItem).draggable();
    _ref = ruleItem.pins;
    for (index in _ref) {
      p = _ref[index];
      pin = p.pin;
      htmlItem.append($("<div id='" + p.pinid + "' class='pin " + pin.direction + "' title='" + pin.description + "'>"));
    }
    $("#rule-editor").append(htmlItem);
    return htmlItem;
  };

  drawLine = function(from, to) {
    var context, x, y;
    context = $('#draw-area')[0].getContext('2d');
    context.beginPath();
    x = from.offset().left;
    y = from.offset().top;
    context.moveTo(x, y);
    x = to.offset().left * ratioX;
    y = to.offset().top * ratioY;
    context.lineTo(x, y);
    context.lineWidth = 1;
    context.strokeStyle = "#ff0000";
    return context.stroke();
  };

}).call(this);
