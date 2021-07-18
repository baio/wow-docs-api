import { Injectable } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { AppTagsSelectorWorkspaceComponent } from '../components/tags-selector-workspace/tags-selector-workspace.component';

@Injectable()
export class TagsSelectorService {
    constructor(private readonly modalController: ModalController) {}

    async selectTag(selectedTags: string[]) {
        const modal = await this.modalController.create({
            component: AppTagsSelectorWorkspaceComponent,
            componentProps: {
                selectedTags,
            },
        });
        await modal.present();
        const { data } = await modal.onWillDismiss();
        return data && data.tag;
    }
}
