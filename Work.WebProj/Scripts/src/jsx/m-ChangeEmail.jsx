//主表單
var GirdForm = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return {
			pram:{receiveMails:''}
		};  
	},
	getDefaultProps:function(){
		return{	
			PName:'pram',
			updatePathName:gb_approot+'Base/ChangeParm/aj_MasterUpdate',
			initPathName:gb_approot+'Base/ChangeParm/aj_Init'

		};
	},	
	componentWillMount:function(){
		//在輸出前觸發，只執行一次如果您在這個方法中呼叫 setState() ，會發現雖然 render() 再次被觸發了但它還是只執行一次。
	},
	componentDidMount:function(){
		//只在客戶端執行一次，當渲染完成後立即執行。當生命週期執行到這一步，元件已經俱有 DOM 所以我們可以透過 this.getDOMNode() 來取得 DOM 。
		//如果您想整和其他 Javascript framework ，使用 setTimeout, setInterval, 或者是發動 AJAX 請在這個方法中執行這些動作。
		this.getAjaxInitData();//載入init資料
	},
	componentWillReceiveProps:function(nextProps){
		//當元件收到新的 props 時被執行，這個方法在初始化時並不會被執行。使用的時機是在我們使用 setState() 並且呼叫 render() 之前您可以比對 props，舊的值在 this.props，而新值就從 nextProps 來。
	},
	shouldComponentUpdate:function(nextProps,nextState){
		/*
		如同其命名，是用來判斷元件是否該更新，當 props 或者 state 變更時會再重新 render 之前被執行。這個方法在初始化時不會被執行，或者當您使用了 forceUpdate 也不會被執行。
		當你確定改變的 props 或 state 並不需要觸發元件更新時，在這個方法中適當的回傳 false 可以提升一些效能。

		shouldComponentUpdate: function(nextProps, nextState) {
  			return nextProps.id !== this.props.id;
		}

		如果 shouldComponentUpdate 回傳 false 則 render() 就會完全被跳過直到下一次 state 改變，此外 componentWillUpdate 和 componentDidUpdate 將不會被觸發。
		當 state 產生異動，為了防止一些奇妙的 bug 產生，預設 shouldComponentUpdate 永遠回傳 true ，不過如果您總是使用不可變性(immutable)的方式來使用 state，並且只在 render 讀取它們那麼你可以複寫 shouldComponentUpdate
		或者是當效能遇到瓶頸，特別是需要處理大量元件時，使用 shouldComponentUpdate 通常能有效地提升速度。
		*/
		return true;
	},
	componentWillUpdate:function(nextProps,nextState){
		/*
			當收到 props 或者 state 立即執行，這個方法在初始化時不會被執行，使用時機通常是在準備更新之前。
			注意您不能在這個方法中使用 this.setState()。如果您需要在修改 props 之後更新 state 請使用 componentWillReceiveProps 取代
		*/
	},
	componentDidUpdate:function(prevProps, prevState){
		/*
			在元件更新之後執行。這個方法同樣不在初始化時執行，使用時機為當元件被更新之後需要執行一些操作。
		*/
	},
	componentWillUnmount:function(){
		//元件被從 DOM 卸載之前執行，通常我們在這個方法清除一些不再需要地物件或 timer。
	},
	handleSubmit: function(e) {
		e.preventDefault();

			jqPost(this.props.updatePathName,this.state.pram)
			.done(function(data, textStatus, jqXHRdata) {
				if(data.result){
					tosMessage(null,'修改完成',1);
				}else{
					alert(data.message);
				}
			}.bind(this))
			.fail(function( jqXHR, textStatus, errorThrown ) {
				showAjaxError(errorThrown);
			});	
		return;
	},
	changePValue:function(name,e){
		this.setInputValue(this.props.PName,name,e);
	},
	setInputValue:function(collentName,name,e){

		var obj = this.state[collentName];
		if(e.target.value=='true'){
			obj[name] = true;
		}else if(e.target.value=='false'){
			obj[name] = false;
		}else{
			obj[name] = e.target.value;
		}
		this.setState({pram:obj});
	},
	getAjaxInitData:function(){
		jqGet(this.props.initPathName)
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({pram:data});
			//載入 參數
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	render: function() {
		var outHtml = null;

			var pram = this.state.pram;

			outHtml =
			(
			<div>
				<ul className="breadcrumb">
					<li><i className="fa-list-alt"></i> {this.props.menuName}</li>
				</ul>
				<h3 className="title">
					{this.props.caption}
				</h3>
				<form className="form-horizontal" onSubmit={this.handleSubmit}>

					<div className="alert alert-warning" role="alert">
						<ol>
							<li>多筆信箱請用「<strong className="text-danger">,</strong>」逗號分開。<br />ex. <strong>user1@demo.com.tw, user2@demo.com.tw</strong></li>
							<li>Email 前面可填收件人姓名，用「<strong className="text-danger">:</strong>」冒號分隔姓名和信箱，此項非必要，可省略。<br />ex. <strong>收件人A:user1@demo.com.tw, 收件人B:user2@demo.com.tw</strong></li>
						</ol>
					</div>

					
					    <div className="form-group">
					        <label className="col-xs-1 control-label">收件信箱</label>
					        <div className="col-xs-9">
					            <input className="form-control" type="text"
					            		value={pram.receiveMails}
					            		onChange={this.changePValue.bind(this,'receiveMails')}
					            		maxLength="500"/>
					        </div>
					    </div>
					

					<div className="form-action">
						<div className="col-xs-4 col-xs-offset-1">
							<button type="submit" className="btn-primary"><i className="fa-check"></i> 儲存</button>
						</div>
					</div>
				</form>
			</div>
			);
		return outHtml;
	}
});