import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { ConfigService } from '../../services/config.service';
import { LoggerService } from '../../services/logger.service';
import { TokenService } from '../../services/token.service';

@Component({
  selector: 'app-datatransfer',
  templateUrl: './datatransfer.component.html'
})
export class DatatransferComponent implements OnInit {

  constructor( private authService: AuthService, private logger: LoggerService, private tokenService: TokenService) { }

  ngOnInit() {
    const config = {
      core: {
        showUploadDropzone: true,
        paginationRppOptions: [5, 10, 25],
        preprocessHashEnabled: false,
        preprocessHashChecked: false,
        parseMessageCallback: (message: any) => {
          try {
            const result = JSON.parse(message);
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
        totalChunksParameterName: 'totalChunks',
        headers: function() {
          const that = this as DatatransferComponent;
          return that.getHeaders;
        }.bind(this)
      }
    };
    const event = new CustomEvent('github:niklr/angular-material-datatransfer.create', { detail: config });
    document.dispatchEvent(event);
  }

  private get getHeaders(): any {
    if (this.authService.isAuthenticated && this.authService.isExpired) {
      this.authService.refresh(false).then(() => {
      }, (e) => {
        this.logger.error(e);
      });
    }

    const bearerHeader = {
      Authorization: `Bearer ${this.tokenService.getAccessToken()}`,
    };
    return bearerHeader;
  }
}
