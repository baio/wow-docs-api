import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Output,
} from '@angular/core';
import { ImageService } from '../../services/image.service';

@Component({
    selector: 'app-upload-image-button',
    templateUrl: 'upload-image-button.component.html',
    styleUrls: ['upload-image-button.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppUploadImageButtonComponent {
    @Output() fileSelected = new EventEmitter<File>();

    constructor(private readonly imageService: ImageService) {}

    async onFileSelected(file: File) {
        //const result = await this.imageService.resizeImageMax(file, 250);
        this.fileSelected.emit(file);
    }
}
