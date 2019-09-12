SurveyCreator
    .StylesManager
    .applyTheme("bootstrap");

var creatorOptions = {
 // hide the embeded survey tab. It is hidden by default
 showEmbededSurveyTab : false,
 // hide the property grid on the right. It is shown by default.
 showPropertyGrid: false,
 // hide the "Options" button menu. It is hidden by default 
 showOptions: false,
 // true means every change triggers a save - false for readonly
 isAutoSave: false,
 // others
 showJSONEditorTab: false,
 showPagesToolbox: false,
};
var surveyCreator = new SurveyCreator.SurveyCreator("creatorElement", creatorOptions);

