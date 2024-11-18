import os
import re

def gen_plant_uml():
    command = 'puml-gen'
    input_path = os.path.join(os.getcwd(), 'Rebirth', 'Assets', 'Scripts')
    output_path = os.path.join(os.getcwd(), 'PlantUML')
    options = ['-dir']
    
    os.makedirs(output_path, exist_ok=True)  # Ensure the output directory exists
    os.system(f'{command} {input_path} {output_path} {" ".join(options)}')

def merge_class_name_with_baseclass(class_name, baseclass_name='MonoBehaviour'):
    if ":" not in class_name:
        # first time merging
        class_name = class_name.replace('"', '').strip()
        return f'"{class_name} : {baseclass_name}"'
    
    # already merged once. append the baseclass to the existing class name
    class_name = class_name.replace('"', '').strip()
    class_name, base_classes = class_name.split(" : ")
    base_classes = base_classes.split(", ")
    base_classes.append(baseclass_name)
    base_classes = ", ".join(base_classes)
    return f'"{class_name} : {base_classes}"'

def filter_baseclass(baseclass_name='MonoBehaviour'):
    plantuml_dir = os.path.join(os.getcwd(), 'PlantUML')
    inheritance_pattern = re.compile(rf'^\s*{baseclass_name}\s*<\|--\s*(.+)\s*$')

    for root, dirs, files in os.walk(plantuml_dir):
        for file in files:
            if file.endswith('.puml'):
                file_path = os.path.join(root, file)
                with open(file_path, 'r', encoding='utf-8') as f:
                    lines = f.readlines()

                modified = False
                classes_to_modify = []

                for line in lines:
                    match = inheritance_pattern.match(line)
                    if match:
                        class_name = match.group(1)
                        classes_to_modify.append(class_name)
                        modified = True

                if modified:
                    lines = [line for line in lines if not inheritance_pattern.match(line)]

                    # for idx, line in enumerate(lines):
                    #     for class_name in classes_to_modify:
                    #         if class_name in line:
                    #             lines[idx] = line.replace(class_name, merge_class_name_with_baseclass(class_name, baseclass_name))

                    with open(file_path, 'w', encoding='utf-8') as f:
                        f.writelines(lines)
                    print(f"Processed and modified file: {file_path}")
                else:
                    print(f"No {baseclass_name} inheritance found in file: {file_path}")

def main():
    target_baseclasses = ["MonoBehaviour", "ScriptableObject"]
    
    print("Generating PlantUML files...")
    gen_plant_uml()
    print("Generation complete.\nProcessing .puml files...")
    for baseclass in target_baseclasses:
        filter_baseclass(baseclass)
    print("Processing complete.")

if __name__ == '__main__':
    main()
