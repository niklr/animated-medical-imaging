import { KeyedCollection, IKeyedCollection } from '../extensions';

export class IdentityModel {
  public sub: string;
  public username: string;

  private rolesCollection: IKeyedCollection<string> = new KeyedCollection<string>();

  public constructor(init?: Partial<IdentityModel>) {
    Object.assign(this, init);
  }

  public get roles(): string[] {
    return this.rolesCollection.values();
  }

  public set roles(roles: string[]) {
    this.rolesCollection = new KeyedCollection<string>();
    if (roles) {
      roles.forEach((role) => {
        this.rolesCollection.add(role, role);
      });
    }
  }

  public isInRole(role: string) {
    return this.rolesCollection.containsKey(role);
  }
}
