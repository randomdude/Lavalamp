(function() {
  var ruleItemOptsDialogFactory;

  ruleItemOptsDialogFactory = (function() {

    ruleItemOptsDialogFactory.name = 'ruleItemOptsDialogFactory';

    function ruleItemOptsDialogFactory() {}

    ruleItemOptsDialogFactory.prototype.getDialog = function(typedDialogName) {
      $('#options-form');
      switch (typedDialogName) {
        case "Weather":
      }
    };

    return ruleItemOptsDialogFactory;

  })();

}).call(this);
