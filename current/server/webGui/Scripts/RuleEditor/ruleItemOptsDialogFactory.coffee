class ruleItemOptsDialogFactory
	getDialog: (typedDialogName) ->
		$('#options-form')
		switch typedDialogName
			when "Weather" then
				