"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g = Object.create((typeof Iterator === "function" ? Iterator : Object).prototype);
    return g.next = verb(0), g["throw"] = verb(1), g["return"] = verb(2), typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (g && (g = 0, op[0] && (_ = 0)), _) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
var App = /** @class */ (function () {
    function App() {
    }
    App.registerJsBridge = function (dotnetObj) {
        App.jsBridgeObj = dotnetObj;
    };
    App.showDiagnostic = function () {
        var _a;
        return (_a = App.jsBridgeObj) === null || _a === void 0 ? void 0 : _a.invokeMethodAsync('ShowDiagnostic');
    };
    App.publishMessage = function (message, payload) {
        var _a;
        return (_a = App.jsBridgeObj) === null || _a === void 0 ? void 0 : _a.invokeMethodAsync('PublishMessage', message, payload);
    };
    App.getTimeZone = function () {
        return Intl.DateTimeFormat().resolvedOptions().timeZone;
    };
    App.getPushNotificationSubscription = function (vapidPublicKey) {
        return __awaiter(this, void 0, void 0, function () {
            var registration, pushManager, subscription, pushChannel, p256dh, auth;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, navigator.serviceWorker.ready];
                    case 1:
                        registration = _a.sent();
                        if (!registration)
                            return [2 /*return*/, null];
                        pushManager = registration.pushManager;
                        if (pushManager == null)
                            return [2 /*return*/, null];
                        return [4 /*yield*/, pushManager.getSubscription()];
                    case 2:
                        subscription = _a.sent();
                        if (!(subscription == null)) return [3 /*break*/, 4];
                        return [4 /*yield*/, pushManager.subscribe({
                                userVisibleOnly: true,
                                applicationServerKey: vapidPublicKey
                            })];
                    case 3:
                        subscription = _a.sent();
                        _a.label = 4;
                    case 4:
                        pushChannel = subscription.toJSON();
                        p256dh = pushChannel.keys['p256dh'];
                        auth = pushChannel.keys['auth'];
                        return [2 /*return*/, {
                                deviceId: "".concat(p256dh, "-").concat(auth),
                                platform: 'browser',
                                p256dh: p256dh,
                                auth: auth,
                                endpoint: pushChannel.endpoint
                            }];
                }
            });
        });
    };
    ;
    return App;
}());
window.addEventListener('message', handleMessage);
window.addEventListener('load', handleLoad);
window.addEventListener('resize', setCssWindowSizes);
function handleMessage(e) {
    // Enable publishing messages from JavaScript's `window.postMessage` to the C# `PubSubService`.
    if (e.data.key === 'PUBLISH_MESSAGE') {
        App.publishMessage(e.data.message, e.data.payload);
    }
}
function handleLoad() {
    setCssWindowSizes();
    if (window.opener != null) {
        // The IExternalNavigationService is responsible for opening pages in a new window,
        // such as during social sign-in flows. Once the external navigation is complete,
        // and the user is redirected back to the newly opened window,
        // the following code ensures that the original window is notified of where it should navigate next.
        window.opener.postMessage({ key: 'PUBLISH_MESSAGE', message: 'NAVIGATE_TO', payload: window.location.href });
        setTimeout(function () { return window.close(); }, 100);
    }
}
function setCssWindowSizes() {
    document.documentElement.style.setProperty('--win-width', "".concat(window.innerWidth, "px"));
    document.documentElement.style.setProperty('--win-height', "".concat(window.innerHeight, "px"));
}
;
BitTheme.init({
    system: true,
    persist: true,
    onChange: function (newTheme, oldThem) {
        if (newTheme === 'dark') {
            document.body.classList.add('theme-dark');
            document.body.classList.remove('theme-light');
        }
        else {
            document.body.classList.add('theme-light');
            document.body.classList.remove('theme-dark');
        }
        var primaryBgColor = getComputedStyle(document.documentElement).getPropertyValue('--bit-clr-bg-pri');
        document.querySelector('meta[name=theme-color]').setAttribute('content', primaryBgColor);
    }
});
