(function($) {
    "use strict";
    var wind = $(window);
    $(function() {
        var akpk = {
            navMenuMobile: function() {
                // Mobile Navigation
                if ($('#main-menu-dropdown').length) {
                    var $mobile_nav = $('#main-menu-dropdown').clone().prop({
                        id: 'mobile-nav'
                    });
                    $mobile_nav.find('> ul').attr({
                        'class': '',
                        'id': ''
                    });
                    $('body').append($mobile_nav);
                    $('body').prepend('<button type="button" id="mobile-nav-toggle"><i class="fa fa-bars"></i></button>');
                    $('body').append('<div id="mobile-body-overly"></div>');
                    $('#mobile-nav').find('.menu-has-children').prepend('<i class="fa fa-chevron-down"></i>');

                    $(document).on('click', '.menu-has-children i', function(e) {
                        $(this).next().toggleClass('menu-item-active');
                        $(this).nextAll('ul').eq(0).slideToggle();
                        $(this).toggleClass("fa-chevron-up fa-chevron-down");
                    });

                    $(document).on('click', '#mobile-nav-toggle', function(e) {
                        $('body').toggleClass('mobile-nav-active');
                        $('#mobile-nav-toggle i').toggleClass('fa-times fa-bars');
                        $('#mobile-body-overly').toggle();
                    });

                    $(document).click(function(e) {
                        var container = $("#mobile-nav, #mobile-nav-toggle");
                        if (!container.is(e.target) && container.has(e.target).length === 0) {
                            if ($('body').hasClass('mobile-nav-active')) {
                                $('body').removeClass('mobile-nav-active');
                                $('#mobile-nav-toggle i').toggleClass('fa-times fa-bars');
                                $('#mobile-body-overly').fadeOut();
                            }
                        }
                    });
                } else if ($("#mobile-nav, #mobile-nav-toggle").length) {
                    $("#mobile-nav, #mobile-nav-toggle").hide();
                }

            },

            smoothScroll: function() {
                $('.navbar-nav a, #mobile-nav a, .scrollto').on('click', function() {
                    if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
                        var target = $(this.hash);
                        if (target.length) {
                            var top_space = 0;

                            if ($('#header').length) {
                                top_space = $('#header').outerHeight();

                                if (!$('#header').hasClass('header-fixed')) {
                                    top_space = top_space - 20;
                                }
                            }

                            $('html, body').animate({
                                scrollTop: target.offset().top - top_space
                            }, 1500, 'easeInOutExpo');

                            if ($(this).parents('.navbar-nav').length) {
                                $('.navbar-nav .menu-active').removeClass('menu-active');
                                $(this).closest('li').addClass('menu-active');
                            }

                            if ($('body').hasClass('mobile-nav-active')) {
                                $('body').removeClass('mobile-nav-active');
                                $('#mobile-nav-toggle i').toggleClass('fa-times fa-bars');
                                $('#mobile-body-overly').fadeOut();
                            }
                            return false;
                        }
                    }
                });

            },

            toggleMainMenu: function() {
                $("#menu-collapse").on('show.bs.collapse', function() {
                    $(".menu-admin .wrapper-menu").addClass("open");
                    $(".header__content").addClass('has-border');
                    $(".header-container").css({
                        'height': '128',
                        'transition': 'all 0.4s ease-out 0s'
                    });

                }).on('hide.bs.collapse', function() {
                    $(".menu-admin .wrapper-menu").removeClass("open");
                    $(".header__content").removeClass('has-border');
                    $(".header-container").css({
                        'height': '64',
                        'transition': 'all 0.4s ease-in 0s'
                    });
                });

            },

            //configure the select2 custom plugin
            selectOption: function() {
                $('.select2').select2({
                    minimumResultsForSearch: Infinity
                });

                $('.select2-area').select2({
                    placeholder: 'Select Area'
                });

                //Multiple select
                $('.js-example-basic-multiple').select2({
                    placeholder: 'Select an option'
                });

                //trigger the value of multiple select
                // $('.js-example-basic-multiple').on('select2:selecting select2:unselecting', function(e) {
                //     var value = e.params.args.data.id;
                //     alert(value);
                // });

            },

            dateSelect: function() {
                $(".dateSelect").flatpickr({
                    // mode: "range",
                    dateFormat: "d/m/Y",
                    wrap: true
                });

                $(".timeSelect").flatpickr({
                    enableTime: true,
                    noCalendar: true,
                    dateFormat: "H:i"
                });
            },

            //for add more event objective
            addButton: function() {
                var eventObjectivehtml = $("#eventobjective").html();
                var btnAddMore = $('#btnAddMore');

                btnAddMore.click(function() {
                    $("#eventobjective").append(eventObjectivehtml);
                });

                $('#eventobjective').on("click", ".deleteItem", function() {
                    $(this).closest(".lineItem").remove();
                    // console.log($(this));
                    return;
                });

            },

            //for add more external exhibitors
            addEe: function() {
                var externalExhibitorshtml = $("#externalexhibitors").html();
                var btnAddEe = $('#btnAddExhibitor');

                btnAddEe.click(function() {
                    $("#externalexhibitors").append(externalExhibitorshtml);
                });

                $('#externalexhibitors').on("click", ".deleteItem", function() {
                    $(this).closest(".lineItem").remove();
                    // console.log($(this));
                    return;
                });

            },

            //for add more add speaker
            addSpeaker: function() {
                var speakerhtml = $("#addspeaker").html();
                var btnAddSpeaker = $('#btnAddspeaker');

                btnAddSpeaker.click(function() {
                    $("#addspeaker").append(speakerhtml);
                });

                $('#addspeaker').on("click", ".deleteItem", function() {
                    $(this).closest(".lineItem").remove();
                    // console.log($(this));
                    return;
                });

            },

            addLanguage: function() {
                var languagehtml = $("#addlanguage").html();
                var btnAddLanguage = $('#btnAddlanguage');

                btnAddLanguage.click(function() {
                    $("#addlanguage").append(languagehtml);
                });

                $("#addlanguage .lineItem").remove();
                $('#addlanguage').on("click", ".deleteItem", function() {
                    $(this).closest(".lineItem").remove();
                    // console.log($(this));
                    return;
                });

            },

            //list file browse input
            updateList: function() {
                var input = document.getElementById('inputfile');
                var output = document.getElementById('fileList');

                $('#inputfile').change(function() {
                    output.innerHTML = '<ul>';
                    for (var i = 0; i < input.files.length; ++i) {
                        output.innerHTML += '<li>' + input.files.item(i).name + '</li>';
                    }
                    output.innerHTML += '</ul>';
                    $('#fileList').wrapAll("<div class='list-file-container'></div>");

                });
            },

            filter: function() {
                $('body').on('click', '.filter.dropdown .cycle-element', function(e) {
                    $(this).parent().toggleClass('open');
                });
                $('body').on('click', function(e) {
                    if (!$('.filter.dropdown').is(e.target) && $('.filter.dropdown').has(e.target).length === 0 && $('.open').has(e.target).length === 0) {
                        $('.filter.dropdown').removeClass('open');
                    }
                });
            },

            //select event category list
            selectCategoryEvent: function() {
                var boxEvent = $("#selecteventcategory .card");

                boxEvent.click(function() {
                    $("#selecteventcategory .box-contain .card").removeClass('active');
                    $(this).addClass('active');
                }); 

            }

           


        };


        akpk.navMenuMobile();
        akpk.smoothScroll();
        akpk.toggleMainMenu();
        akpk.selectOption();
        akpk.dateSelect();
        akpk.addButton();
        akpk.addEe();
        akpk.addSpeaker();
        akpk.addLanguage();
        akpk.updateList();
        akpk.filter();
        akpk.selectCategoryEvent();
       

    });


})(jQuery);