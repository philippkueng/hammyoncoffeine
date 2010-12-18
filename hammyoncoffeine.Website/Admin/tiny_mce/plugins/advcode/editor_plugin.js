(function() {
	tinymce.PluginManager.requireLangPack('advcode'); 
	tinymce.create('tinymce.plugins.AdvancedCodeEditorPlugin', {
    	init : function(ed, url) {
			// Register commands
			ed.addCommand('AdvCodeEditor', function() {
				ed.windowManager.open({
					file : url + '/advcode.html',
					width : 720 + parseInt(ed.getLang('advcode.delta_width', 0)),
					height : 600 + parseInt(ed.getLang('advcode.delta_height', 0)),
					inline : 1
				}, {
					plugin_url : url
				});
			});
			// Register buttons
			ed.addButton('advcode', {
				title : 'advcode.desc',
				cmd : 'AdvCodeEditor',
				image : url + '/img/advcode.gif'
			});
			ed.onNodeChange.add(function(ed, cm, n) {});
    	},
		getInfo : function() {
			return {
				longname : 'Advanced Code Editor',
				author : 'Ryan Demmer',
				authorurl : 'http://www.joomlacontenteditor.net',
				infourl : 'http://www.joomlacontenteditor.net',
				version : '1.5.0'
			};
		}
	});
  	// Register plugin
	tinymce.PluginManager.add('advcode', tinymce.plugins.AdvancedCodeEditorPlugin);
})();
