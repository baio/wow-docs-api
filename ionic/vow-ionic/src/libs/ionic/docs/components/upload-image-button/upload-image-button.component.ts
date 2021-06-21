import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Output,
} from '@angular/core';
import { ImageService } from '../../services/image.service';
import { Camera, CameraResultType } from '@capacitor/camera';

@Component({
    selector: 'app-upload-image-button',
    templateUrl: 'upload-image-button.component.html',
    styleUrls: ['upload-image-button.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppUploadImageButtonComponent {
    @Output() fileSelected = new EventEmitter<string>();

    constructor() {}

    async onClick(fileInput: HTMLInputElement) {

        try {
            const image = await Camera.getPhoto({
                quality: 90,
                //allowEditing: true,
                resultType: CameraResultType.Base64,
                saveToGallery: false,
                preserveAspectRatio: true,
                width: 1500,
            });

            // image.webPath will contain a path that can be set as an image src.
            // You can access the original file using image.path, which can be
            // passed to the Filesystem API to read the raw data of the image,
            // if desired (or pass resultType: CameraResultType.Base64 to getPhoto)

            this.fileSelected.emit(
                `data:image/${image.format};base64,${image.base64String}`
            );
        } catch (err) {
            console.warn(err);
        }
    }

    async onFileSelected(file: File) {}
}
