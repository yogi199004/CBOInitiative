using L10n.Web.API.Controllers;
using L10N.API.Contracts;
using L10N.API.Model;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace L10n.Web.API.Tests
{
    [TestClass]
    public class L10NControllerTests
    {
        private readonly Mock<ICosmosService> _cosmosSvc = new Mock<ICosmosService>();
        private readonly Mock<IAPIService> _apiSvc = new Mock<IAPIService>();
        private readonly Mock<ILogger<L10NController>> _logTest = new Mock<ILogger<L10NController>>();


        [TestMethod]
        public async Task GetMetaData_WithValidApplicationName_ReturnsMetadata()
        {
            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            string _res = @"[{
                        'language': 'English (United States)',
                'localeCode': 'en-US',
                'nativeName': 'English (United States)',
                'englishName': 'English (United States)',
                'nativeLanguageName': 'English',
                'englishLanguageName': 'English',
                'nativeCountryName': 'United States',
                'englishCountryName': 'United States',
                'lastModifiedDate': '2019-10-15T11:25:23.833'
            }]";

            List<MetaDataModel> _response = JsonConvert.DeserializeObject<List<MetaDataModel>>(_res);
            List<IEnumerable<MetaDataModel>> result = new List<IEnumerable<MetaDataModel>>();
            result.Add(_response);
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result);

            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            var response = await _TestController.Object.GetMetaData("omnia");

            Assert.IsNotNull(response);
            Assert.AreEqual(200, response.StatusCode);

        }


        [TestMethod]
        public async Task GetMetaData_BlankApplicationName_ReturnsBadRequest()
        {

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            List<IEnumerable<MetaDataModel>> result = new List<IEnumerable<MetaDataModel>>();

            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result);

            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            var response = await _TestController.Object.GetMetaData("");

            Assert.IsFalse(response.StatusCode == 200);

        }

        [TestMethod]
        public async Task GetMetaData_WithInValidApplicationName_ReturnsNoMetadata()
        {

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            List<IEnumerable<MetaDataModel>> result = new List<IEnumerable<MetaDataModel>>();

            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result);

            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            var response = await _TestController.Object.GetMetaData("abc");

            Assert.IsFalse(response.StatusCode == 200);

        }


        [TestMethod]
        public async Task GetAllKeyValues_WithValidApplicationNameAndLocale_ReturnsKeys()
        {

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            string _res = @"[{
                        'language': 'English (United States)',
                'localeCode': 'en-US',
                'nativeName': 'English (United States)',
                'englishName': 'English (United States)',
                'nativeLanguageName': 'English',
                'englishLanguageName': 'English',
                'nativeCountryName': 'United States',
                'englishCountryName': 'United States',
                'lastModifiedDate': '2019-10-15T11:25:23.833'
            }]";

            List<MetaDataModel> _response = JsonConvert.DeserializeObject<List<MetaDataModel>>(_res);
            List<IEnumerable<MetaDataModel>> result1 = new List<IEnumerable<MetaDataModel>>();
            result1.Add(_response);
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result1);
            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);
            Dictionary<string, string> keyvalues = new Dictionary<string, string>();
            keyvalues.Add("Mobile.AlertDetail.ButtonViewAlert.Text", "View");
            keyvalues.Add("Mobile.Alert.TaskUpdated.Prefix", "Updated Task");

            AllKeyValues allKeyValues = new AllKeyValues();
            allKeyValues.keyValues = keyvalues;

            KeyValues item = new KeyValues();
            item.hasData = false;
            item.AllKeyValues = allKeyValues;

            List<KeyValues> result = new List<KeyValues>();
            result.Add(item);

            Task<List<KeyValues>> _keyValuesData = Task.FromResult(result);
            _apiSvc.Setup(x => x.getAppKeyValuesData(It.IsAny<Container>(), It.IsAny<string>(), It.IsAny<string>())).Returns(() => _keyValuesData);

            var response = await _TestController.Object.GetAllKeyValues("Connect", "en-US");

            Assert.IsTrue(response.StatusCode == 200);

        }

        [TestMethod]
        public async Task GetAllKeyValues_WithInValidApplicationName_ReturnsNoData()
        {

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            List<IEnumerable<MetaDataModel>> result1 = new List<IEnumerable<MetaDataModel>>();
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result1);
            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            List<KeyValues> result = new List<KeyValues>();

            Task<List<KeyValues>> _keyValuesData = Task.FromResult(result);
            _apiSvc.Setup(x => x.getAppKeyValuesData(It.IsAny<Container>(), It.IsAny<string>(), It.IsAny<string>())).Returns(() => _keyValuesData);

            var response = await _TestController.Object.GetAllKeyValues("ab12", "en-US");

            Assert.IsFalse(response.StatusCode == 200);

        }

        [TestMethod]
        public async Task GetAllKeyValues_WithInValidLocale_ReturnsNoData()
        {

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            List<IEnumerable<MetaDataModel>> result1 = new List<IEnumerable<MetaDataModel>>();
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result1);
            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            List<KeyValues> result = new List<KeyValues>();

            Task<List<KeyValues>> _keyValuesData = Task.FromResult(result);
            _apiSvc.Setup(x => x.getAppKeyValuesData(It.IsAny<Container>(), It.IsAny<string>(), It.IsAny<string>())).Returns(() => _keyValuesData);

            var response = await _TestController.Object.GetAllKeyValues("Connect", "123");

            Assert.IsFalse(response.StatusCode == 200);

        }

        [TestMethod]
        public async Task GetAllKeyValues_WithBlankAppAndLocale_ReturnsNoData()
        {

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            List<IEnumerable<MetaDataModel>> result1 = new List<IEnumerable<MetaDataModel>>();
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result1);
            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            List<KeyValues> result = new List<KeyValues>();

            Task<List<KeyValues>> _keyValuesData = Task.FromResult(result);
            _apiSvc.Setup(x => x.getAppKeyValuesData(It.IsAny<Container>(), It.IsAny<string>(), It.IsAny<string>())).Returns(() => _keyValuesData);

            var response = await _TestController.Object.GetAllKeyValues("", "");

            Assert.IsFalse(response.StatusCode == 200);

        }

        [TestMethod]
        public async Task GetValueByKey_WithBlankParameters_ReturnsBadRequestError()
        {

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            List<IEnumerable<MetaDataModel>> result1 = new List<IEnumerable<MetaDataModel>>();
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result1);
            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            List<KeyValues> result = new List<KeyValues>();

            Task<List<KeyValues>> _keyValuesData = Task.FromResult(result);
            _apiSvc.Setup(x => x.getAppKeyValuesData(It.IsAny<Container>(), It.IsAny<string>(), It.IsAny<string>())).Returns(() => _keyValuesData);

            var response = await _TestController.Object.GetValueByKey("", "", "");

            Assert.IsFalse(response.StatusCode == 200);

        }

        [TestMethod]
        public async Task GetValueByKey_WithInvalidParameters_ReturnsBadRequestError()
        {

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            List<IEnumerable<MetaDataModel>> result1 = new List<IEnumerable<MetaDataModel>>();
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result1);
            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            List<KeyValues> result = new List<KeyValues>();

            Task<List<KeyValues>> _keyValuesData = Task.FromResult(result);
            _apiSvc.Setup(x => x.getAppKeyValuesData(It.IsAny<Container>(), It.IsAny<string>(), It.IsAny<string>())).Returns(() => _keyValuesData);

            var response = await _TestController.Object.GetValueByKey("Count", "tel", "key11");

            Assert.IsFalse(response.StatusCode == 200);

        }

        [TestMethod]
        public async Task GetValueByKey_WithValidParameters_ReturnsKeyValueData()
        {

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            string _res = @"[{
                        'language': 'English (United States)',
                'localeCode': 'en-US',
                'nativeName': 'English (United States)',
                'englishName': 'English (United States)',
                'nativeLanguageName': 'English',
                'englishLanguageName': 'English',
                'nativeCountryName': 'United States',
                'englishCountryName': 'United States',
                'lastModifiedDate': '2019-10-15T11:25:23.833'
            }]";

            List<MetaDataModel> _response = JsonConvert.DeserializeObject<List<MetaDataModel>>(_res);
            List<IEnumerable<MetaDataModel>> result1 = new List<IEnumerable<MetaDataModel>>();
            result1.Add(_response);
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result1);

            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            Dictionary<string, string> keyvalues = new Dictionary<string, string>();
            keyvalues.Add("Mobile.AlertDetail.ButtonViewAlert.Text", "View");
            keyvalues.Add("Mobile.Alert.TaskUpdated.Prefix", "Updated Task");

            AllKeyValues allKeyValues = new AllKeyValues();
            allKeyValues.keyValues = keyvalues;

            KeyValues item = new KeyValues();
            item.hasData = false;
            item.AllKeyValues = allKeyValues;

            List<KeyValues> result = new List<KeyValues>();
            result.Add(item);

            Task<List<KeyValues>> _keyValuesData = Task.FromResult(result);
            _apiSvc.Setup(x => x.getAppKeyValuesData(It.IsAny<Container>(), It.IsAny<string>(), It.IsAny<string>())).Returns(() => _keyValuesData);

            var response = await _TestController.Object.GetValueByKey("Connect", "en-US", "Mobile.AlertDetail.ButtonViewAlert.Text");

            Assert.IsTrue(response.StatusCode == 200);

        }

        [TestMethod]
        public async Task GetValueByKey_WithInvalidKey_ReturnsBadRequestError()
        {

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            List<IEnumerable<MetaDataModel>> result1 = new List<IEnumerable<MetaDataModel>>();
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result1);
            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            Dictionary<string, string> keyvalues = new Dictionary<string, string>();
            keyvalues.Add("Mobile.AlertDetail.ButtonViewAlert.Text", "View");
            keyvalues.Add("Mobile.Alert.TaskUpdated.Prefix", "Updated Task");

            AllKeyValues allKeyValues = new AllKeyValues();
            allKeyValues.keyValues = keyvalues;

            KeyValues item = new KeyValues();
            item.hasData = false;
            item.AllKeyValues = allKeyValues;

            List<KeyValues> result = new List<KeyValues>();
            result.Add(item);

            Task<List<KeyValues>> _keyValuesData = Task.FromResult(result);
            _apiSvc.Setup(x => x.getAppKeyValuesData(It.IsAny<Container>(), It.IsAny<string>(), It.IsAny<string>())).Returns(() => _keyValuesData);

            var response = await _TestController.Object.GetValueByKey("Connect", "en-US", "MobileTestKey");

            Assert.IsFalse(response.StatusCode == 200);

        }

        [TestMethod]
        public async Task GetAllKeys_WithValidApplicationNameAndLocale_ReturnsKeys()
        {

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            string _res = @"[{
                        'language': 'English (United States)',
                'localeCode': 'en-US',
                'nativeName': 'English (United States)',
                'englishName': 'English (United States)',
                'nativeLanguageName': 'English',
                'englishLanguageName': 'English',
                'nativeCountryName': 'United States',
                'englishCountryName': 'United States',
                'lastModifiedDate': '2019-10-15T11:25:23.833'
            }]";

            List<MetaDataModel> _response = JsonConvert.DeserializeObject<List<MetaDataModel>>(_res);
            List<IEnumerable<MetaDataModel>> result1 = new List<IEnumerable<MetaDataModel>>();
            result1.Add(_response);
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result1);

            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            Dictionary<string, string> keyvalues = new Dictionary<string, string>();
            keyvalues.Add("Mobile.AlertDetail.ButtonViewAlert.Text", "View");
            keyvalues.Add("Mobile.Alert.TaskUpdated.Prefix", "Updated Task");

            AllKeyValues allKeyValues = new AllKeyValues();
            allKeyValues.keyValues = keyvalues;

            KeyValues item = new KeyValues();
            item.hasData = false;
            item.AllKeyValues = allKeyValues;

            List<KeyValues> result = new List<KeyValues>();
            result.Add(item);

            Task<List<KeyValues>> _keyValuesData = Task.FromResult(result);
            _apiSvc.Setup(x => x.getAppKeyValuesData(It.IsAny<Container>(), It.IsAny<string>(), It.IsAny<string>())).Returns(() => _keyValuesData);

            var response = await _TestController.Object.GetAllKeys("Connect", "en-US");

            Assert.IsTrue(response.StatusCode == 200);

        }

        [TestMethod]
        public async Task GetAllKeys_WithInValidApplicationName_ReturnsNoData()
        {

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            List<IEnumerable<MetaDataModel>> result1 = new List<IEnumerable<MetaDataModel>>();
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result1);
            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            List<KeyValues> result = new List<KeyValues>();

            Task<List<KeyValues>> _keyValuesData = Task.FromResult(result);
            _apiSvc.Setup(x => x.getAppKeyValuesData(It.IsAny<Container>(), It.IsAny<string>(), It.IsAny<string>())).Returns(() => _keyValuesData);

            var response = await _TestController.Object.GetAllKeys("ab12", "en-US");

            Assert.IsFalse(response.StatusCode == 200);

        }

        [TestMethod]
        public async Task GetAllKeys_WithInValidLocale_ReturnsNoData()
        {

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            List<IEnumerable<MetaDataModel>> result1 = new List<IEnumerable<MetaDataModel>>();
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result1);
            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            List<KeyValues> result = new List<KeyValues>();

            Task<List<KeyValues>> _keyValuesData = Task.FromResult(result);
            _apiSvc.Setup(x => x.getAppKeyValuesData(It.IsAny<Container>(), It.IsAny<string>(), It.IsAny<string>())).Returns(() => _keyValuesData);

            var response = await _TestController.Object.GetAllKeys("Connect", "123");

            Assert.IsFalse(response.StatusCode == 200);

        }

        [TestMethod]
        public async Task GetAllKeys_WithBlankAppAndLocale_ReturnsNoData()
        {

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            List<IEnumerable<MetaDataModel>> result1 = new List<IEnumerable<MetaDataModel>>();
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result1);
            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            List<KeyValues> result = new List<KeyValues>();

            Task<List<KeyValues>> _keyValuesData = Task.FromResult(result);
            _apiSvc.Setup(x => x.getAppKeyValuesData(It.IsAny<Container>(), It.IsAny<string>(), It.IsAny<string>())).Returns(() => _keyValuesData);

            var response = await _TestController.Object.GetAllKeys("", "");

            Assert.IsFalse(response.StatusCode == 200);

        }

        [TestMethod]
        public async Task GetValuesByKeyList_WithValidParameters_ReturnsListOfKeyValues()
        {
            string[] keys = new string[2] { "Mobile.Project.EmptySearch.Message", "Mobile.TaskFilter.DueDateItem.Name" };
            KeyList keyList = new KeyList
            {
                AppName = "Connect",
                locale = "en-US",
                keys = keys
            };
            string _res = @"[{
                        'language': 'English (United States)',
                'localeCode': 'en-US',
                'nativeName': 'English (United States)',
                'englishName': 'English (United States)',
                'nativeLanguageName': 'English',
                'englishLanguageName': 'English',
                'nativeCountryName': 'United States',
                'englishCountryName': 'United States',
                'lastModifiedDate': '2019-10-15T11:25:23.833'
            }]";

            List<MetaDataModel> _response = JsonConvert.DeserializeObject<List<MetaDataModel>>(_res);
            List<IEnumerable<MetaDataModel>> result1 = new List<IEnumerable<MetaDataModel>>();
            result1.Add(_response);
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result1);

            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            Dictionary<string, string> keyvalues = new Dictionary<string, string>();
            keyvalues.Add("Mobile.Project.EmptySearch.Message", "No matching results found.\nTry searching with different\nparameters.");
            keyvalues.Add("Mobile.TaskFilter.DueDateItem.Name", "Due Date");

            AllKeyValues allKeyValues = new AllKeyValues();
            allKeyValues.keyValues = keyvalues;

            KeyValues item = new KeyValues();
            item.hasData = false;
            item.AllKeyValues = allKeyValues;

            List<KeyValues> result = new List<KeyValues>();
            result.Add(item);

            Task<List<KeyValues>> _keyValuesData = Task.FromResult(result);

            _apiSvc.Setup(x => x.getAppKeyValuesData(It.IsAny<Container>(), It.IsAny<string>(), It.IsAny<string>())).Returns(() => _keyValuesData);


            var response = await _TestController.Object.GetValuesByKeyList(keyList);

            Assert.IsTrue(response.StatusCode == 200);
        }

        [TestMethod]
        public async Task GetValuesByKeyList_WithEmptyParameters_ReturnsNoData()
        {
            string[] keys = new string[2] { "", "" };
            KeyList keyList = new KeyList
            {
                AppName = "",
                locale = "",
                keys = keys
            };

            List<IEnumerable<MetaDataModel>> result1 = new List<IEnumerable<MetaDataModel>>();
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result1);
            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            List<KeyValues> result = new List<KeyValues>();

            Task<List<KeyValues>> _keyValuesData = Task.FromResult(result);

            _apiSvc.Setup(x => x.getAppKeyValuesData(It.IsAny<Container>(), It.IsAny<string>(), It.IsAny<string>())).Returns(() => _keyValuesData);


            var response = await _TestController.Object.GetValuesByKeyList(keyList);

            Assert.IsFalse(response.StatusCode == 200);
        }

        [TestMethod]
        public async Task GetValuesByKeyList_WithInvalidParameters_ReturnsNoData()
        {
            string[] keys = new string[2] { "abc", "abc" };
            KeyList keyList = new KeyList
            {
                AppName = "abc",
                locale = "xyz",
                keys = keys
            };

            List<IEnumerable<MetaDataModel>> result1 = new List<IEnumerable<MetaDataModel>>();
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result1);
            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            List<KeyValues> result = new List<KeyValues>();

            Task<List<KeyValues>> _keyValuesData = Task.FromResult(result);

            _apiSvc.Setup(x => x.getAppKeyValuesData(It.IsAny<Container>(), It.IsAny<string>(), It.IsAny<string>())).Returns(() => _keyValuesData);


            var response = await _TestController.Object.GetValuesByKeyList(keyList);

            Assert.IsFalse(response.StatusCode == 200);
        }

        [TestMethod]
        public async Task GetValuesByKeyPattern_WithValidParameters_ReturnsListOfKeyValues()
        {
            string[] keys = new string[2] { "Mobile.Project.EmptySearch*", "Mobile.TaskFilter.*" };
            KeyModel keyPattren = new KeyModel
            {
                AppName = "Connect",
                Locale = "en-US",
                KeysPattern = keys
            };

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            string _res = @"[{
                        'language': 'English (United States)',
                'localeCode': 'en-US',
                'nativeName': 'English (United States)',
                'englishName': 'English (United States)',
                'nativeLanguageName': 'English',
                'englishLanguageName': 'English',
                'nativeCountryName': 'United States',
                'englishCountryName': 'United States',
                'lastModifiedDate': '2019-10-15T11:25:23.833'
            }]";

            List<MetaDataModel> _response = JsonConvert.DeserializeObject<List<MetaDataModel>>(_res);
            List<IEnumerable<MetaDataModel>> result1 = new List<IEnumerable<MetaDataModel>>();
            result1.Add(_response);
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result1);

            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            Dictionary<string, string> keyvalues = new Dictionary<string, string>();
            keyvalues.Add("Mobile.Project.EmptySearch.Message", "No matching results found.\nTry searching with different\nparameters.");
            keyvalues.Add("Mobile.TaskFilter.DueDateItem.Name", "Due Date");
            keyvalues.Add("Mobile.TaskFilter.HeadeSortBy.Title", "Sort By");

            AllKeyValues allKeyValues = new AllKeyValues();
            allKeyValues.keyValues = keyvalues;

            KeyValues item = new KeyValues();
            item.hasData = false;
            item.AllKeyValues = allKeyValues;

            List<KeyValues> result = new List<KeyValues>();
            result.Add(item);

            Task<List<KeyValues>> _keyValuesData = Task.FromResult(result);

            _apiSvc.Setup(x => x.getAppKeyValuesData(It.IsAny<Container>(), It.IsAny<string>(), It.IsAny<string>())).Returns(() => _keyValuesData);


            var response = await _TestController.Object.GetValuesByKeyPattern(keyPattren);

            Assert.IsTrue(response.StatusCode == 200);
        }

        [TestMethod]
        public async Task GetValuesByKeyPattern_WithEmptyParameters_ReturnsNoData()
        {
            string[] keys = new string[2] { "", "" };
            KeyModel keyPattren = new KeyModel
            {
                AppName = "",
                Locale = "",
                KeysPattern = keys
            };

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            List<IEnumerable<MetaDataModel>> result1 = new List<IEnumerable<MetaDataModel>>();
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result1);
            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            List<KeyValues> result = new List<KeyValues>();

            Task<List<KeyValues>> _keyValuesData = Task.FromResult(result);

            _apiSvc.Setup(x => x.getAppKeyValuesData(It.IsAny<Container>(), It.IsAny<string>(), It.IsAny<string>())).Returns(() => _keyValuesData);


            var response = await _TestController.Object.GetValuesByKeyPattern(keyPattren);

            Assert.IsFalse(response.StatusCode == 200);
        }

        [TestMethod]
        public async Task GetValuesByKeyPattern_WithInvalidParameters_ReturnsNoData()
        {
            string[] keys = new string[2] { "abc", "abc" };
            KeyModel keyPattren = new KeyModel
            {
                AppName = "abc",
                Locale = "xyz",
                KeysPattern = keys
            };

            var _TestController = new Mock<L10NController>(_cosmosSvc.Object, _apiSvc.Object, _logTest.Object) { CallBase = true };

            List<IEnumerable<MetaDataModel>> result1 = new List<IEnumerable<MetaDataModel>>();
            Task<List<IEnumerable<MetaDataModel>>> _metaData = Task.FromResult(result1);
            _apiSvc.Setup(x => x.getAppMetaData(It.IsAny<Container>(), It.IsAny<string>())).Returns(() => _metaData);

            List<KeyValues> result = new List<KeyValues>();

            Task<List<KeyValues>> _keyValuesData = Task.FromResult(result);

            _apiSvc.Setup(x => x.getAppKeyValuesData(It.IsAny<Container>(), It.IsAny<string>(), It.IsAny<string>())).Returns(() => _keyValuesData);


            var response = await _TestController.Object.GetValuesByKeyPattern(keyPattren);

            Assert.IsFalse(response.StatusCode == 200);
        }
    }
}