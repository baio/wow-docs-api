import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { AlertController, ModalController } from '@ionic/angular';
import { Store } from '@ngrx/store';
import Fuse from 'fuse.js';
import { BehaviorSubject, combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Tag } from '../../models';
import { createTag } from '../../ngrx/actions';
import { selectTagsAsSortedList } from '../../ngrx/selectors';

export interface TagsSelectorWorkspaceView {
    tags: string[];
}

export const searchTags = (tags: Tag[], search: string) => {
    if (search) {
        search = search.toLowerCase();

        const options = {
            includeScore: true,
            keys: [
                {
                    name: 'name',
                    weight: 0.9,
                },
            ],
        };
        const fuse = new Fuse(tags, options);
        const searchRes = fuse.search(search);
        const res = searchRes.map((m) => m.item);
        return res;
    } else {
        return tags;
    }
};

@Component({
    selector: 'app-tags-selector-workspace',
    templateUrl: 'tags-selector-workspace.component.html',
    styleUrls: ['tags-selector-workspace.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppTagsSelectorWorkspaceComponent {
    private readonly search$ = new BehaviorSubject<string>(null);
    readonly view$: Observable<TagsSelectorWorkspaceView>;

    @Input() selectedTags: string[];
    constructor(
        private readonly store: Store,
        private readonly modalController: ModalController,
        private readonly alertController: AlertController
    ) {
        const tags$ = store.select(selectTagsAsSortedList);
        this.view$ = combineLatest([tags$, this.search$]).pipe(
            map(([tags, search]) => searchTags(tags, search)),
            map((tags) => tags.map((m) => m.name)),
            map((tags) => ({ tags }))
        );
    }

    trackByTag(_, tag: string) {
        return tag;
    }

    onSelect(tag: string) {
        this.modalController.dismiss({ tag });
    }

    async onCreate() {
        const alert = await this.alertController.create({
            header: 'Создать новый таг',
            inputs: [
                {
                    name: 'name',
                    type: 'text',
                    placeholder: 'Имя тага',
                },
            ],
            buttons: [
                {
                    text: 'Отмена',
                    role: 'cancel',
                    cssClass: 'secondary',
                    handler: (blah) => {},
                },
                {
                    text: 'Создать',
                    handler: (x) => {
                        if (x.name) {
                            this.store.dispatch(
                                createTag({
                                    name: x.name,
                                    date: new Date().getTime(),
                                })
                            );
                        }
                    },
                },
            ],
        });

        const res = await alert.present();
    }
}
