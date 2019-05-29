import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-datatransfer',
  templateUrl: './datatransfer.component.html',
  styleUrls: ['./datatransfer.component.scss']
})
export class DatatransferComponent implements OnInit {

  constructor() { }

  ngOnInit() {
    var config = {
      core: {
        showUploadDropzone: true,
        paginationRppOptions: [5, 10, 25],
        preprocessHashEnabled: false,
        preprocessHashChecked: false
      },
      resumablejs: {
        target: '/Home/ResumableUpload',
        testChunks: false,
        simultaneousUploads: 1,
        maxChunkRetries: 6,
        chunkRetryInterval: 5000
      }
    };
    var event = new CustomEvent('github:niklr/angular-material-datatransfer.create', { 'detail': config });
    document.dispatchEvent(event);
  }

}
