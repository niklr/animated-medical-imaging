import { Component, OnInit } from '@angular/core';
import { ConfigService } from '../../services/config.service';

@Component({
  selector: 'app-datatransfer',
  templateUrl: './datatransfer.component.html'
})
export class DatatransferComponent implements OnInit {

  constructor() { }

  ngOnInit() {
    var config = {
      core: {
        showUploadDropzone: true,
        paginationRppOptions: [5, 10, 25],
        preprocessHashEnabled: false,
        preprocessHashChecked: false,
        parseMessageCallback: function (message) {
          try {
            var result = JSON.parse(message);
            return result.id;
          } catch (e) {
            return message;
          }
        }
      },
      resumablejs: {
        target: ConfigService.options.apiEndpoint + '/objects/upload',
        chunkSize: 5 * 1024 * 1024,
        testChunks: false,
        simultaneousUploads: 1,
        maxChunkRetries: 6,
        chunkRetryInterval: 5000,
        chunkNumberParameterName: 'chunkNumber',
        chunkSizeParameterName: 'chunkSize',
        currentChunkSizeParameterName: 'currentChunkSize',
        totalSizeParameterName: 'totalSize',
        typeParameterName: 'resumableType',
        identifierParameterName: 'uid',
        fileNameParameterName: 'filename',
        relativePathParameterName: 'relativePath',
        totalChunksParameterName: 'totalChunks'
      }
    };
    var event = new CustomEvent('github:niklr/angular-material-datatransfer.create', { 'detail': config });
    document.dispatchEvent(event);
  }
}
