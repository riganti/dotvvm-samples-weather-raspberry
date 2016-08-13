ko.bindingHandlers["dotvvm-Morris"] = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        var options = ko.unwrap(allBindings()["dotvvm-Morris-Series"]);
        
        element.morrisGraph = Morris.Line({
            element: element,
            data: [],
            xkey: options.xMemberName,
            ykeys: options.yMemberNames,
            ymin: options.yMin,
            ymax: options.yMax,
            dateFormat: function (d) { return dotvvm.globalize.formatString(options.dateFormat, new Date(d)) },
            yLabelFormat: function (v) { return dotvvm.globalize.formatString(options.yLabelFormat, v) },
            postUnit: options.postUnit,
            labels: options.labels
        });

        dotvvm.events.afterPostback.subscribe(function() {
            ko.bindingHandlers["dotvvm-Morris"].update.call(this, element, valueAccessor);
        });
    },
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        var data = dotvvm.serialization.serialize(ko.unwrap(valueAccessor()), { serializeAll: true });
        element.morrisGraph.setData(data);
    }
};