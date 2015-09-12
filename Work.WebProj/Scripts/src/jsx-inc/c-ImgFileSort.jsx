//後端圖片上傳
var MasterImageUpload = React.createClass({
	mixins: [SortableMixin], 
	getInitialState: function() {  
		return {
			filelist:[]
		};  
	},
	getDefaultProps:function(){
		return{	
			url_upload:null,
			url_list:null,
			url_delete:null,
			url_download:null,
			url_sort:null,
			FileKind:null,
			MainId:0,
			uploader:null
		};
	},
	componentDidUpdate:function(prevProps, prevState){
		// if(this.props.ParentEditType==2){
		// 	this.createFileUpLoadObject();
		// }

	},
	componentDidMount:function(){
		if(this.props.MainId>0){
			this.createFileUpLoadObject();
			this.getFileList();
		}
	},
	componentWillReceiveProps:function(nextProps){
		
	},
	componentWillUnmount:function(){
		console.log('MasterFileUpload','destroy');
		if(this.props.uploader!=null){
			this.props.uploader.destroy();
		}
	},
	deleteFile:function(filename){
		jqPost(this.props.url_delete,{
			id:this.props.MainId,
			fileKind:this.props.FileKind,
			filename:filename
		})			
		.done(function(data, textStatus, jqXHRdata) {
			if(data.result){
				this.getFileList();
			}else{
				alert(data.message);
			}
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	getFileList:function(){
		jqPost(this.props.url_list,{
			id:this.props.MainId,
			fileKind:this.props.FileKind
		})			
		.done(function(data, textStatus, jqXHRdata) {
			if(data.result){
				this.setState({filelist:data.files})
			}else{
				alert(data.message);
			}
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	createFileUpLoadObject:function(){
			var btn = document.getElementById('upload-btn-' + this.props.MainId +'-'+this.props.FileKind);
			var r_this = this;

		  	this.props.uploader = new ss.SimpleUpload({
		        button: btn,
		        url: this.props.url_upload,
		        data:{
		        	id:this.props.MainId,
		        	fileKind:this.props.FileKind
		        },
		        name: 'fileName',
		        multiple: true,
		        maxSize: 5000,
		        allowedExtensions: ['jpg', 'jpeg', 'png','gif'],
		        accept: 'image/*',
		        responseType: 'json',
				onSubmit: function(filename, ext) {            
					if(r_this.props.MainId==0){
						alert('此筆資料未完成新增，無法上傳檔案!')
						return false;
					}

					var progress = document.createElement('div'), // container for progress bar
						bar = document.createElement('div'), // actual progress bar
						fileSize = document.createElement('div'), // container for upload file size
						wrapper = document.createElement('div'), // container for this progress bar
						progressBox = document.getElementById('progressBox-' + r_this.props.MainId); // on page container for progress bars

					// Assign each element its corresponding class
					progress.className = 'progress';
					bar.className = 'progress-bar progress-bar-success progress-bar-striped active';            
					fileSize.className = 'size';
					wrapper.className = 'wrapper';

					// Assemble the progress bar and add it to the page
					progress.appendChild(bar); 
					wrapper.innerHTML = '<div class="name">'+filename+'</div>'; // filename is passed to onSubmit()
					wrapper.appendChild(fileSize);
					wrapper.appendChild(progress);                                       
					progressBox.appendChild(wrapper); // just an element on the page to hold the progress bars    

					// Assign roles to the elements of the progress bar
					this.setProgressBar(bar); // will serve as the actual progress bar
					this.setFileSizeBox(fileSize); // display file size beside progress bar
					this.setProgressContainer(wrapper); // designate the containing div to be removed after upload	
				},
	
				onSizeError: function() {
					//errBox.innerHTML = 'Files may not exceed 500K.';
					alert('檔案大小請勿超過5MB(5000KB)!!.');
				},
				onExtError: function() {
					//errBox.innerHTML = 'Invalid file type. Please select a PNG, JPG, GIF image.';
				},
		        onComplete: function(file, response) {
		        	if(response.result){ 
						r_this.getFileList();
					}else{
						alert(response.error);
					}
		        }
			});
	},
	sortableOptions: {
        ref: "SortImage",
        model:'filelist',
        group: "shared"
    },
    handleSort: function (evt) { 

    	var parms = [];
		for (var i in this.state.filelist) {
			var item = this.state.filelist[i];

			var file_object = 
			{
				fileName:item.fileName,
				sort:i+1
			};

			parms.push(file_object);
		}

		jqPost(this.props.url_sort,{
			id:this.props.MainId,
			fileKind:this.props.FileKind,
			file_object:parms
		})			
		.done(function(data, textStatus, jqXHRdata) {
			if(data.result){
				//this.getFileList();
			}else{
				alert(data.message);
			}
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	render: function() {

		var outHtml = null;
		var imgButtonHtml=null;
		if (this.props.ParentEditType==1) {
			imgButtonHtml=(
				<div className="form-control">
				<small className="col-xs-6 help-inline">請先按儲存後方可上傳圖片</small>
				</div>
				);
		}else if(this.props.ParentEditType==2){
			imgButtonHtml=(
				<div className="form-control">
				<input type="file" id={'upload-btn-' + this.props.MainId +'-'+this.props.FileKind} accept="image/*" />
				</div>
				);
		};
		outHtml=(				
		<div>
			{imgButtonHtml}
			<p className="help-block list-group" ref="SortImage">
			{
				this.state.filelist.map(function(itemData,i) {
					var  subOutHtml =
					<span className="img-upload list-group-item" key={i}>
						<button type="button" 
						className="close" 
						onClick={this.deleteFile.bind(this,itemData.fileName)}
						title="刪除圖片">&times;</button>
						<img src={itemData.iconPath} title={formatFileSize(itemData.size) } />
					</span>;
					return subOutHtml;
				},this)
			}
			</p>
			<div id={'progressBox-' + this.props.MainId} className="progress-wrap"></div>
		</div>
		);
		return outHtml;
	}
});